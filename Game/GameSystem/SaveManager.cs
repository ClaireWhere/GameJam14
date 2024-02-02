using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameJam14.Game.GameSystem;
internal class SaveManager {
    public SaveManager() {
        this.Save1 = new SaveData(Data.EntityData.Player, 5);
        this.Save2 = null;
        this.Save3 = null;
        this.State = SavingState.Ready;
        this.SaveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Clairora Games", "GameJam14");
    }

    public enum ErrorState {
        None,
        SaveFileDoesNotExist,
        FileOpen,
        SaveManagerNotReady,
        StreamError
    }

    public enum SaveSlot {
        One,
        Two,
        Three
    }

    public enum SavingState {
        Saving,
        Serializing,
        Ready,
        LoadingDirectory,
        LoadingStream,
        LoadingFile,
        Deserializing,
        Finishing
    }

    public SaveData CurrentSave {
        get {
            return this.CurrentSaveSlot == SaveSlot.One
                ? this.Save1
                : this.CurrentSaveSlot == SaveSlot.Two ? this.Save2 : this.CurrentSaveSlot == SaveSlot.Three ? this.Save3 : null;
        }
        set {
            if ( this.CurrentSaveSlot == SaveSlot.One ) {
                this.Save1 = value;
            } else if ( this.CurrentSaveSlot == SaveSlot.Two ) {
                this.Save2 = value;
            } else if ( this.CurrentSaveSlot == SaveSlot.Three ) {
                this.Save3 = value;
            }
        }
    }

    public SavingState State { get; private set; }
    public async Task<ErrorState> Load() {
        if ( this.State != SavingState.Ready ) {
            this.CurrentSave = null;
            return ErrorState.SaveManagerNotReady;
        }

        this.State = SavingState.LoadingDirectory;

        if ( !Directory.Exists(this.SaveDir) || !File.Exists(path: this.SavePath) ) {
            this.State = SavingState.Ready;
            return ErrorState.SaveFileDoesNotExist;
        }

        this.State = SavingState.LoadingStream;

        try {
            await using FileStream loadStream = File.OpenRead(this.SavePath);

            if ( loadStream == null ) {
                this.State = SavingState.Ready;
                this.CurrentSave = null;
                return ErrorState.StreamError;
            }

            this.State = SavingState.Deserializing;

            JsonSerializerOptions options = new JsonSerializerOptions {
                WriteIndented = true,
                AllowTrailingCommas = false,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            this.CurrentSave = await JsonSerializer.DeserializeAsync<SaveData>(
                utf8Json: loadStream,
                options: options
            );

            this.State = SavingState.Finishing;

            await loadStream.DisposeAsync();
        } catch ( Exception e ) {
            Debug.WriteLine(e.Message);
            this.State = SavingState.Ready;
            return ErrorState.FileOpen;
        }

        this.State = SavingState.Ready;
        return ErrorState.None;
    }

    public async Task<ErrorState> Save() {
        if ( this.State != SavingState.Ready ) {
            return ErrorState.SaveManagerNotReady;
        }

        this.State = SavingState.LoadingDirectory;
        Debug.WriteLine("Loading Save Directory");

        if ( !Directory.Exists(this.SaveDir) ) {
            Directory.CreateDirectory(this.SaveDir);
        }

        this.State = SavingState.LoadingStream;

        try {
            Debug.WriteLine("Loading Stream");

            /*
             * Note: .NET versions below 8.0 do not support the JsonObjectCreationHandling option
             * Ideally, we would use:
             *   JsonObjectCreationHandling = System.Text.Json.Serialization.JsonObjectCreationHandling.Replace
             *
             * This means that the serializer will attempt to populate existing JSON objects rather than
             *   creating new ones. This causes issues with saving to the file, as floating point precision
             *   differences between the previous file and the new data may cause the JSON serializer to
             *   output invalid JSON.
             *
             * The current fix is to clear the contents of the file before writing to it. This is not ideal,
             *   but it works.
            */
            File.WriteAllText(this.SavePath, string.Empty);

            await using FileStream saveStream = File.OpenWrite(this.SavePath);

            if ( saveStream == null ) {
                this.State = SavingState.Ready;
                this.CurrentSave = null;
                return ErrorState.StreamError;
            }

            this.State = SavingState.Serializing;
            Debug.WriteLine("Serializing Save Data");

            JsonSerializerOptions options = new JsonSerializerOptions {
                WriteIndented = true,
                AllowTrailingCommas = false,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict
            };
            await JsonSerializer.SerializeAsync(utf8Json: saveStream, value: this.CurrentSave, inputType: this.CurrentSave.GetType(), options: options);

            Debug.WriteLine("Save data serialized and saved. Data:");

            this.State = SavingState.Saving;
            await saveStream.DisposeAsync();

            Debug.WriteLine(File.ReadAllText(this.SavePath));
        } catch ( Exception e ) {
            Debug.WriteLine(e.Message);
            this.State = SavingState.Ready;
            return ErrorState.FileOpen;
        }

        this.State = SavingState.Ready;
        return ErrorState.None;
    }

    public bool SaveLoaded() {
        return this.CurrentSave != null;
    }

    public void SelectSaveSlot(SaveSlot saveSlot) {
        this.CurrentSaveSlot = saveSlot;
    }

    public void Update(SaveData currentSave) {
        this.CurrentSave = currentSave;
    }

    private SaveSlot CurrentSaveSlot { get; set; }
    private SaveData Save1 { get; set; }
    private SaveData Save2 { get; set; }
    private SaveData Save3 { get; set; }
    private string SaveDir { get; set; }
    private string SavePath {
        get {
            return Path.Combine(this.SaveDir, "save" + (int)this.CurrentSaveSlot + ".json");
        }
    }
}

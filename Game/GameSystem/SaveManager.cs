using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameJam14.Game.GameSystem;
internal class SaveManager {
    public SaveManager() {
        Save1 = new SaveData(Data.EntityData.Player, 5);
        Save2 = null;
        Save3 = null;
        State = SavingState.Ready;
        SaveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Clairora Games", "GameJam14");
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
            if ( CurrentSaveSlot == SaveSlot.One ) {
                return Save1;
            } else if ( CurrentSaveSlot == SaveSlot.Two ) {
                return Save2;
            } else if ( CurrentSaveSlot == SaveSlot.Three ) {
                return Save3;
            } else {
                return null;
            }
        }
        set {
            if ( CurrentSaveSlot == SaveSlot.One ) {
                Save1 = value;
            } else if ( CurrentSaveSlot == SaveSlot.Two ) {
                Save2 = value;
            } else if ( CurrentSaveSlot == SaveSlot.Three ) {
                Save3 = value;
            }
        }
    }

    public SavingState State { get; private set; }
    public async Task<ErrorState> Load() {
        if ( State != SavingState.Ready ) {
            CurrentSave = null;
            return ErrorState.SaveManagerNotReady;
        }

        State = SavingState.LoadingDirectory;

        if ( !Directory.Exists(SaveDir) || !File.Exists(path: SavePath) ) {
            State = SavingState.Ready;
            return ErrorState.SaveFileDoesNotExist;
        }

        State = SavingState.LoadingStream;

        try {
            await using FileStream loadStream = File.OpenRead(SavePath);

            if ( loadStream == null ) {
                State = SavingState.Ready;
                CurrentSave = null;
                return ErrorState.StreamError;
            }

            State = SavingState.Deserializing;

            var options = new JsonSerializerOptions {
                WriteIndented = true,
                AllowTrailingCommas = false,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            CurrentSave = await JsonSerializer.DeserializeAsync<SaveData>(
                utf8Json: loadStream,
                options: options
            );

            State = SavingState.Finishing;

            await loadStream.DisposeAsync();
        } catch ( Exception e ) {
            Debug.WriteLine(e.Message);
            State = SavingState.Ready;
            return ErrorState.FileOpen;
        }

        State = SavingState.Ready;
        return ErrorState.None;
    }

    public async Task<ErrorState> Save() {
        if ( State != SavingState.Ready ) {
            return ErrorState.SaveManagerNotReady;
        }

        State = SavingState.LoadingDirectory;
        Debug.WriteLine("Loading Save Directory");

        if ( !Directory.Exists(SaveDir) ) {
            Directory.CreateDirectory(SaveDir);
        }

        State = SavingState.LoadingStream;

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
            File.WriteAllText(SavePath, string.Empty);

            await using FileStream saveStream = File.OpenWrite(SavePath);

            if ( saveStream == null ) {
                State = SavingState.Ready;
                CurrentSave = null;
                return ErrorState.StreamError;
            }

            State = SavingState.Serializing;
            Debug.WriteLine("Serializing Save Data");

            var options = new JsonSerializerOptions {
                WriteIndented = true,
                AllowTrailingCommas = false,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict
            };
            await JsonSerializer.SerializeAsync(utf8Json: saveStream, value: CurrentSave, inputType: CurrentSave.GetType(), options: options);

            Debug.WriteLine("Save data serialized and saved. Data:");

            State = SavingState.Saving;
            await saveStream.DisposeAsync();

            Debug.WriteLine(File.ReadAllText(SavePath));
        } catch ( Exception e ) {
            Debug.WriteLine(e.Message);
            State = SavingState.Ready;
            return ErrorState.FileOpen;
        }

        State = SavingState.Ready;
        return ErrorState.None;
    }

    public bool SaveLoaded() {
        return CurrentSave != null;
    }

    public void SelectSaveSlot(SaveSlot saveSlot) {
        CurrentSaveSlot = saveSlot;
    }

    public void Update(SaveData currentSave) {
        CurrentSave = currentSave;
    }

    private SaveSlot CurrentSaveSlot { get; set; }
    private SaveData Save1 { get; set; }
    private SaveData Save2 { get; set; }
    private SaveData Save3 { get; set; }
    private string SaveDir { get; set; }
    private string SavePath {
        get {
            return Path.Combine(SaveDir, "save" + (int) CurrentSaveSlot + ".json");
        }
    }
}

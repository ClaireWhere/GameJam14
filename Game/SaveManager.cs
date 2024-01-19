using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace GameJam14.Game;
internal class SaveManager {
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
    public enum SaveSlot {
        One,
        Two,
        Three
    }

    public enum ErrorState {
        None,
        SaveFileDoesNotExist,
        FileOpen,
        SaveManagerNotReady,
        StreamError
    }

    private SaveData Save1 { get; set; }
    private SaveData Save2 { get; set; }
    private SaveData Save3 { get; set; }
    private SaveSlot CurrentSaveSlot { get; set; }
    public SavingState State { get; private set; }
    private string SaveDir { get; set; }

    public SaveManager() {
        Save1 = new SaveData(Data.EntityData.Player, 5);
        Save2 = null;
        Save3 = null;
        State = SavingState.Ready;
        SaveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Clairora Games", "GameJam14");
    }

    public void Update(SaveData currentSave) {
        this.CurrentSave = currentSave;
    }

    public void SelectSaveSlot(SaveSlot saveSlot) {
        CurrentSaveSlot = saveSlot;
    }

    public async Task<ErrorState> Load() {
        if ( State != SavingState.Ready ) {
            this.CurrentSave = null;
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
                this.CurrentSave = null;
                return ErrorState.StreamError;
            }

            State = SavingState.Deserializing;

            this.CurrentSave = await JsonSerializer.DeserializeAsync<SaveData>(loadStream);

            State = SavingState.Finishing;

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
        if (State != SavingState.Ready) {
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

            await using FileStream saveStream = File.OpenWrite(SavePath);

            if ( saveStream == null ) {
                State = SavingState.Ready;
                this.CurrentSave = null;
                return ErrorState.StreamError;
            }

            State = SavingState.Serializing;
            Debug.WriteLine("Serializing Save Data");

            var options = new JsonSerializerOptions {
                WriteIndented = true
            };
            await JsonSerializer.SerializeAsync(utf8Json: saveStream, value: this.CurrentSave, inputType: this.CurrentSave.GetType(), options: options);

            Debug.WriteLine("Save data serialized and saved. Data:");

            State = SavingState.Saving;
            await saveStream.DisposeAsync();

            // Debug.WriteLine(File.ReadAllText(this.SavePath));
        } catch ( Exception e ) {
            Debug.WriteLine(e.Message);
            this.State = SavingState.Ready;
            return ErrorState.FileOpen;
        }

        State = SavingState.Ready;
        return ErrorState.None;
    }

    public bool SaveLoaded() {
        return CurrentSave != null;
    }

    private string SavePath {
        get {
            return Path.Combine(SaveDir, "save" + (int) CurrentSaveSlot + ".json");
        }
    }

    private SaveData CurrentSave {
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
}

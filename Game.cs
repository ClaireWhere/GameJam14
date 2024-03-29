﻿using GameJam14.Game.Data;
using GameJam14.Game.Entity;
using GameJam14.Game.GameSystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GameJam14;
internal class Game2 : Microsoft.Xna.Framework.Game {
    public GraphicsDeviceManager Graphics;

    public Screen Screen { get; private set; }
    public Camera Camera { get; private set; }

    public float RandomFloat(float min, float max) {
        return ( (float)this._random.NextDouble() * ( max - min ) ) + min;
    }

    public int RandomInt(int min, int max) {
        return this._random.Next(min, max);
    }

    private EntityManager _entityManager;
    private SaveManager _saveManager;

    private SaveData _currentSave;

    private readonly Random _random = new Random();

    private bool _isPaused;
    private bool _isSaving;

    private static Game2 s_Instance;
    public static Game2 Instance() {
        s_Instance ??= new Game2();
        return s_Instance;
    }

    public SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Game" /> class.
    /// </summary>
    private Game2() {
        this.Graphics = new GraphicsDeviceManager(this);
        this.Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
        this._isSaving = false;
        this._isPaused = false;
        this._currentSave = null;
    }

    /// <summary>
    ///   Initializes the Game.
    /// </summary>
    protected override void Initialize() {
        this.Graphics.PreferredBackBufferWidth = 1920;
        this.Graphics.PreferredBackBufferHeight = 1080;
        this.Graphics.ApplyChanges();
        this.Window.AllowUserResizing = true;
        this.Window.ClientSizeChanged += (sender, args) => {
            this.Graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            this.Graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            this.Graphics.ApplyChanges();
        };

        this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
        this._entityManager = new EntityManager();

        this.Screen = new Screen(1920, 1080);
        this.Camera = new Camera(this.Screen);

        base.Initialize();
    }

    /// <summary>
    ///   Loads the content of the game
    /// </summary>
    protected override void LoadContent() {
        Assets.LoadContent(this.Content);

        // Add entities to queue, then update the entity manager to process the queue
        this._entityManager.AddEntity(Player.Instance);
        this._entityManager.AddEntity(EntityData.Tree);
        this._entityManager.Update(new GameTime());

        this._currentSave = new SaveData(this._entityManager.Player(), 0);
        this._saveManager = new SaveManager();
        this._saveManager.SelectSaveSlot(SaveManager.SaveSlot.One);
        this._saveManager.Update(this._currentSave);
    }

    /// <summary>
    ///   Updates the content of the game
    /// </summary>
    protected override void Update(GameTime gameTime) {
        if ( this._isSaving ) {
            return;
        }

        Input.Update();

        if ( Input.IsKeyPressed(Keys.Escape) ) {
            this._isPaused = !this._isPaused;
        }
#if DEBUG
        if ( Input.IsKeyPressed(Keys.F1) ) {
            _ = this.Save();
        }

        if ( Input.IsKeyPressed(Keys.F2) ) {
            _ = this.LoadSave();
        }

        if ( Input.IsKeyPressed(Keys.F3) ) {
            this._entityManager.Reset();
            this._entityManager.AddEntity(Player.Instance);
            this._entityManager.AddEntity(EntityData.Tree);
        }

        if ( Input.IsKeyDown(Keys.Up) ) {
            Debug.WriteLine("Moving camera up");
            this.Camera.ZoomIn();
        }

        if ( Input.IsKeyDown(Keys.Down) ) {
            this.Camera.ZoomOut();
        }

        if ( Input.IsKeyPressed(Keys.F6) ) {
            Debug.WriteLine(
                "Player position: " + this._entityManager.Player().Position + "\n" +
                "Player velocity: " + this._entityManager.Player().Velocity + "\n" +
                "Player acceleration: " + this._entityManager.Player().Acceleration + "\n" +
                "Player destination: " + this._entityManager.Player().Destination + "\n" +
                "Player is traveling: " + this._entityManager.Player().IsTraveling + "\n" +
                "Player is moving: " + this._entityManager.Player().IsMoving + "\n"
            );

            this.Camera.GetExtents(out Vector2 topLeft, out Vector2 bottomRight, out Vector2 center);

            Debug.WriteLine(
                "Camera position: " + this.Camera.Position + "\n" +
                "Camera zoom: " + this.Camera.Zoom + "\n" +
                "Camera extents: " + topLeft + ", " + bottomRight + "\n" +
                "Camera center: " + center + "\n"
            );
        }

        if ( Input.IsKeyPressed(Keys.F7) ) {
            this._entityManager.Player().TeleportTo(new Vector2(this.Screen.Width / 2, this.Screen.Height / 2));
        }
#endif

        if ( !this._isPaused && !this._isSaving ) {
            if ( Keyboard.GetState().IsKeyDown(Keys.Space) ) {
                Debug.WriteLine("Space pressed");
                this.Draw(gameTime);
            }

            // Update everything here
            this._entityManager.Update(gameTime);
            this.Camera.MoveTo(this._entityManager.Player().Position);
        }

        if ( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) ) {
            base.Update(gameTime);
        }
    }

    public void AddEntity(Entity entity) {
        this._entityManager.AddEntity(entity);
    }
    public void RemoveEntity(Entity entity) {
        this._entityManager.RemoveEntity(entity);
    }

    private async Task Save() {
        if ( this._isSaving ) {
            return;
        }

        this._isSaving = true;
        try {
            this._currentSave.Update(this._entityManager.Player(), 0);
            this._saveManager.Update(this._currentSave);
            await this._saveManager.Save();
        } catch ( Exception e ) {
            Debug.WriteLine($"Error saving: {e}");
        }

        this._isSaving = false;
    }

    private async Task LoadSave() {
        if ( this._isSaving ) {
            return;
        }

        this._isSaving = true;
        try {
            SaveManager.ErrorState state = await this._saveManager.Load();
            if ( state != SaveManager.ErrorState.None ) {
                Debug.WriteLine($"Error loading save: {state}");
            } else {
                this._currentSave = this._saveManager.CurrentSave;
                this._entityManager.Reset();
                this._currentSave.UpdatePlayer();
                this._entityManager.AddEntity(Game.Entity.Player.Instance);
                // Update the rest of save data here
            }
        } catch ( Exception e ) {
            Debug.WriteLine($"Error loading save: {e}");
        }

        this._isSaving = false;
    }

    /// <summary>
    ///   Draws objects on screen
    /// </summary>
    protected override void Draw(GameTime gameTime) {
        this.Screen.Set();
        this.GraphicsDevice.Clear(Color.DarkSlateGray);

        this._entityManager.Draw(this.Camera);

        this.Screen.Unset();
        this.Screen.Present(this.SpriteBatch);

        base.Draw(gameTime);
    }
}

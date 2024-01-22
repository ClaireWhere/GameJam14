using GameJam14.Game.Data;
using GameJam14.Game.Entity;
using GameJam14.Game.GameSystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14;
internal class Game2 : Microsoft.Xna.Framework.Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Screen _screen;
    private Camera _camera;

    private EntityManager _entityManager;
    private SaveManager _saveManager;

    private SaveData _currentSave;

    private bool _isPaused;
    private bool _isSaving;

    private static Game2 s_Instance;
    public static Game2 Instance() {
        if ( s_Instance == null ) {
            s_Instance = new Game2();
        }
        return s_Instance;
    }

    public SpriteBatch SpriteBatch { get { return this._spriteBatch; } }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Game" /> class.
    /// </summary>
    private Game2() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        this._isSaving = false;
        this._isPaused = false;
        this._currentSave = null;
    }

    /// <summary>
    ///   Initializes the Game.
    /// </summary>
    protected override void Initialize() {
        this._graphics.PreferredBackBufferWidth = 1920;
        this._graphics.PreferredBackBufferHeight = 1080;
        this._graphics.ApplyChanges();
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (sender, args) => {
            this._graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            this._graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            this._graphics.ApplyChanges();
        };

        this._spriteBatch = new SpriteBatch(GraphicsDevice);
        this._entityManager = new EntityManager();

        this._screen = new Screen(1920, 1080);
        this._camera = new Camera(this._screen);

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
            _isPaused = !_isPaused;
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
        }

        if ( Input.IsKeyDown(Keys.Up) ) {
            Debug.WriteLine("Moving camera up");
            this._camera.ZoomIn();
        }
        if ( Input.IsKeyDown(Keys.Down) ) {
            this._camera.ZoomOut();
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

            this._camera.GetExtents(out Vector2 topLeft, out Vector2 bottomRight, out Vector2 center);

            Debug.WriteLine(
                "Camera position: " + this._camera.Position + "\n" +
                "Camera zoom: " + this._camera.Zoom + "\n" +
                "Camera extents: " + topLeft + ", " + bottomRight + "\n" +
                "Camera center: " + center + "\n"
            );
        }
        if ( Input.IsKeyPressed(Keys.F7) ) {
            this._entityManager.Player().TeleportTo(new Vector2(this._screen.Width / 2, this._screen.Height / 2));
        }
#endif

        if ( !_isPaused && !_isSaving ) {
            if ( Keyboard.GetState().IsKeyDown(Keys.Space) ) {
                Debug.WriteLine("Space pressed");
                this.Draw(gameTime);
            }

            // Update everything here
            this._entityManager.Update(gameTime);
        }

        if ( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) )
            base.Update(gameTime);
    }

    public void AddEntity(Entity entity) {
        this._entityManager.AddEntity(entity);
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
        this._screen.Set();
        GraphicsDevice.Clear(Color.DarkSlateGray);

        _entityManager.Draw(this._camera);

        this._screen.Unset();
        this._screen.Present(_spriteBatch);

        base.Draw(gameTime);
    }
}

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
internal class Game2 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Screen _screen;

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
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    private Game2()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        this._isSaving = false;
        this._isPaused = false;
    }

    /// <summary>
    /// Initializes the Game.
    /// </summary>
    protected override void Initialize()
    {
        this._graphics.PreferredBackBufferWidth = 1920;
        this._graphics.PreferredBackBufferHeight = 1080;
        this._graphics.ApplyChanges();
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (sender, args) => {
            this._graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            this._graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            this._graphics.ApplyChanges();
        };

        // this.sprites = new List<Sprite>();
        this._entityManager = new EntityManager();

        this._screen = new Screen(1920, 1080);

        base.Initialize();
    }

    /// <summary>
    ///  Loads the content of the game
    /// </summary>
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Assets.LoadContent(this.Content);

        this._entityManager.AddEntity(EntityData.Player);
        this._entityManager.AddEntity(EntityData.Tree);

        this._currentSave = new SaveData((Game.Entity.Player) this._entityManager.GetEntity(0), 0);
        this._saveManager = new SaveManager();
        this._saveManager.SelectSaveSlot(SaveManager.SaveSlot.One);
        this._saveManager.Update(this._currentSave);

        //sprites.Add(SpriteData.PlayerSprite);
        //sprites.Add(SpriteData.TreeSprite);
    }

    /// <summary>
    /// Updates the content of the game
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
        if (this._isSaving) {
            return;
        }
        Input.Update();

        if (Input.IsKeyPressed(Keys.Escape)) {
            _isPaused = !_isPaused;
        }

        if (Input.IsKeyPressed(Keys.F1)) {
            _ = this.Save();
        }

        if (Input.IsKeyPressed(Keys.F2)) {
            _ = this.LoadSave();
        }

        if (!_isPaused && !_isSaving) {
            if ( Keyboard.GetState().IsKeyDown(Keys.Space) ) {
                Console.WriteLine("Space pressed");
                this.Draw(gameTime);
            }

            // Update everything here
            this.ProcessEntityQueue();
            _entityManager.Update(gameTime);
        }

        if ( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) )
            base.Update(gameTime);
    }

    public void AddEntity(Entity entity) {
        this._entityManager.AddEntity(entity);
    }

    private async Task Save() {
        if (this._isSaving) {
            return;
        }
        this._isSaving = true;
        try {
            this._currentSave.Update((Game.Entity.Player) this._entityManager.GetEntity(0), 0);
            this._saveManager.Update(this._currentSave);
            await this._saveManager.Save();
        } catch ( Exception e ) {
            Debug.WriteLine($"Error saving: {e}");
        }
        this._isSaving = false;
    }

    private async Task LoadSave() {
        if (this._isSaving) {
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
    /// Draws objects on screen
    /// </summary>
    protected override void Draw(GameTime gameTime)
    {
        this._screen.Set();
        GraphicsDevice.Clear(Color.DarkSlateGray);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

        //foreach (Sprite sprite in sprites)
        //{
        //    sprite.Draw(_spriteBatch);
        //}

        _entityManager.Draw(_spriteBatch);

        _spriteBatch.End();

        this._screen.Unset();
        this._screen.Present(_spriteBatch);

        base.Draw(gameTime);
    }

}

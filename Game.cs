using GameJam14.Game;
using GameJam14.Game.Data;
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

    // private List<Sprite> sprites;
    private EntityManager _entityManager;
    private SaveManager _saveManager;

    private SaveData _currentSave;

    private bool _isPaused = false;
    private bool _isSaving = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game2()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Initializes the Game.
    /// </summary>
    protected override void Initialize()
    {
        // this.sprites = new List<Sprite>();
        this._entityManager = new EntityManager();

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
            this.Save();
        }

        if (Input.IsKeyPressed(Keys.F2)) {
            _ = this.LoadSave();
        }

        if (!_isPaused) {
            if ( Keyboard.GetState().IsKeyDown(Keys.Space) ) {
                Console.WriteLine("Space pressed");
                this.Draw(gameTime);
            }

            // Update everything here
            _entityManager.Update(gameTime);
        }

        if ( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) )
            base.Update(gameTime);
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
                this._entityManager.AddEntity(this._currentSave.GetPlayer());
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
        GraphicsDevice.Clear(Color.DarkSlateGray);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

        //foreach (Sprite sprite in sprites)
        //{
        //    sprite.Draw(_spriteBatch);
        //}

        _entityManager.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

}

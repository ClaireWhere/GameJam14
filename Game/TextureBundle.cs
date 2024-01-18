using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace GameJam14.Game;
internal class TextureBundle {


    /// <summary>
    /// Gets the textures of this bundle.
    /// </summary>
    private Dictionary<TextureType, Texture2D> _textures { get; }
    /// <summary>
    /// Gets or sets the current texture state of this bundle.
    /// </summary>
    private TextureType _currentState { get; set; }

    public TextureType CurrentState {
        get {
            return this._currentState;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextureBundle"/> class.
    /// </summary>
    /// <param name="textures">The textures.</param>
    /// <param name="initialState">The initial state.</param>
    public TextureBundle(Dictionary<TextureType, Texture2D> textures, TextureType? initialState = null) {
        this._textures = textures;
        this._currentState = initialState ?? textures.Keys.First();
    }

    /// <summary>
    /// Gets the current texture of this bundle.
    /// </summary>
    public Texture2D Texture {
        get {
            return this._textures[this._currentState];
        }
    }

    public void SetState(TextureType state) {
        if (this.HasTexture(state)) {
            this._currentState = state;
        }
    }

    private bool HasTexture(TextureType type) {
        return this._textures.ContainsKey(type);
    }

    private Texture2D GetTexture(TextureType type) {
        if (!this.HasTexture(type)) {
            return null;
        }
        return this._textures[type];
    }

    private Texture2D GetTexture(string type) {
        return this.GetTexture((TextureType) Enum.Parse(typeof(TextureType), type));
    }

    private Texture2D GetTexture(int type) {
        return this.GetTexture((TextureType) type);
    }

    /// <summary>
    /// Sets the texture for the specified type.
    /// </summary>
    /// <param name="type">The texture type.</param>
    /// <param name="texture">The texture.</param>
    public void SetTexture(TextureType type, Texture2D texture) {
        if (this.HasTexture(type)) {
            this._textures[type] = texture;
        } else {
            this._textures.Add(type, texture);
        }
    }

}

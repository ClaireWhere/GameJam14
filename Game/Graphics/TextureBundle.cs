using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;

namespace GameJam14.Game.Graphics;
internal class TextureBundle : IDisposable {
    /// <summary>
    ///   Initializes a new instance of the <see cref="TextureBundle" /> class.
    /// </summary>
    /// <param name="textures">
    ///   The textures.
    /// </param>
    /// <param name="initialState">
    ///   The initial state.
    /// </param>
    public TextureBundle(Dictionary<TextureType, Texture2D> textures, TextureType? initialState = null) {
        this._textures = textures;
        this._currentState = initialState ?? textures.Keys.First();
        this._disposed = false;
    }

    public TextureType CurrentState {
        get {
            return this._currentState;
        }
    }

    /// <summary>
    ///   Gets the current texture of this bundle.
    /// </summary>
    public Texture2D Texture {
        get {
            return this._textures[this._currentState];
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void SetState(TextureType state) {
        if ( this.HasTexture(state) ) {
            this._currentState = state;
        }
    }

    /// <summary>
    ///   Sets the texture for the specified type.
    /// </summary>
    /// <param name="type">
    ///   The texture type.
    /// </param>
    /// <param name="texture">
    ///   The texture.
    /// </param>
    public void SetTexture(TextureType type, Texture2D texture) {
        if ( this.HasTexture(type) ) {
            this._textures[type] = texture;
        } else {
            this._textures.Add(type, texture);
        }
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this._disposed = true;
    }

    /// <summary>
    ///   Gets or sets the current texture state of this bundle.
    /// </summary>
    private TextureType _currentState { get; set; }

    /// <summary>
    ///   Gets the textures of this bundle.
    /// </summary>
    private Dictionary<TextureType, Texture2D> _textures { get; }

    private bool HasTexture(TextureType type) {
        return this._textures.ContainsKey(type);
    }
    private bool _disposed;
}

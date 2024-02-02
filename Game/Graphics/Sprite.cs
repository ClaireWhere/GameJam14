using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Runtime.InteropServices;

namespace GameJam14.Game.Graphics;
internal class Sprite : IDisposable {
	/// <summary>
	///   Initializes a new instance of the <see cref="Sprite" /> class.
	/// </summary>
	/// <param name="textures">
	///   The textures of the sprite.
	/// </param>
	/// <param name="position">
	///   The position of the sprite.
	/// </param>
	/// <param name="rotation">
	///   The rotation of the sprite.
	/// </param>
	/// <param name="scale">
	///   The scale of the sprite.
	/// </param>
	/// <param name="sourceRectangle">
	///   The source rectangle.
	/// </param>
	/// <param name="color">
	///   The color filter to apply to the sprite.
	/// </param>
	/// <param name="effects">
	///   The effects.
	/// </param>
	/// <param name="layerDepth">
	///   The layer depth.
	/// </param>
	public Sprite(
			TextureBundle textures,
			Vector2 position,
			[Optional] float rotation,
			[Optional] float scale,
			[Optional] Vector2 origin,
			[Optional] Rectangle sourceRectangle,
			[Optional] Color color,
			[Optional] SpriteEffects effects,
			[Optional] float layerDepth
		) {
		this.Textures = textures;
		this.Position = position;
		this.Rotation = rotation == default ? 0 : rotation;
		this.Scale = scale == default ? 1 : scale;
		this.Origin = origin == default ? Vector2.Zero : origin;
		this.SourceRectangle = sourceRectangle == default ? new Rectangle(0, 0, this.Texture.Width, this.Texture.Height) : sourceRectangle;
		this.Color = color == default ? Color.White : color;
		this.Effects = effects == default ? SpriteEffects.None : effects;
		this.LayerDepth = layerDepth == default ? 0 : layerDepth;
		this._disposed = true;
	}

	/// <summary>
	///   Gets the color filter of the sprite.
	/// </summary>
	public Color Color { get; set; }

	/// <summary>
	///   Gets the effects of the sprite.
	/// </summary>
	public SpriteEffects Effects { get; set; }

	/// <summary>
	///   Gets the layer depth of the sprite.
	/// </summary>
	public float LayerDepth { get; }

	public Vector2 Origin { get; set; }
	/// <summary>
	///   Gets or sets the position of the sprite.
	/// </summary>
	public Vector2 Position { get; set; }

	/// <summary>
	///   Gets the rotation of the sprite in radians.
	/// </summary>
	public float Rotation { get; set; }

	/// <summary>
	///   Gets the scale of the sprite.
	/// </summary>
	public float Scale { get; set; }

	/// <summary>
	///   Gets the source rectangle of the sprite.
	/// </summary>
	public Rectangle SourceRectangle { get; }

	public Texture2D Texture {
		get {
			return this.Textures.Texture;
		}
	}

	public TextureType TextureType {
		get {
			return this.Textures.CurrentState;
		}
	}

	public void Dispose() {
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	public void SetTexture(TextureType type) {
		if ( this.Textures.CurrentState == type ) {
			return;
		}
		this.Textures.SetState(type);
	}

	protected virtual void Dispose(bool disposing) {
		if ( this._disposed ) {
			return;
		}
		this.Texture.Dispose();
		this.Textures.Dispose();
		this._disposed = true;
	}

	/// <summary>
	///   Gets the texture of the sprite.
	/// </summary>
	private TextureBundle Textures { get; }
	private bool _disposed;
}

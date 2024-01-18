using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game;
internal class Sprite {
    /// <summary>
    /// Initializes a new instance of the <see cref="Sprite"/> class.
    /// </summary>
    /// <param name="textures">The textures of the sprite.</param>
    /// <param name="position">The position of the sprite.</param>
    /// <param name="rotation">The rotation of the sprite.</param>
    /// <param name="scale">The scale of the sprite.</param>
    /// <param name="sourceRectangle">The source rectangle.</param>
    /// <param name="color">The color filter to apply to the sprite.</param>
    /// <param name="effects">The effects.</param>
    /// <param name="layerDepth">The layer depth.</param>
    public Sprite(
            TextureBundle textures,
            Vector2 position,
            [Optional] float rotation,
            [Optional] float scale,
            [Optional] Rectangle sourceRectangle,
            [Optional] Color color,
            [Optional] SpriteEffects effects,
            [Optional] float layerDepth
        ) {
        this.Textures = textures;
        this.Position = position;
        if ( rotation == default ) {
            this.Rotation = 0;
        } else {
            this.Rotation = rotation;
        }
        if ( scale == default ) {
            this.Scale = 1;
        } else {
            this.Scale = scale;
        }
        if ( sourceRectangle == default ) {
            this.SourceRectangle = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);
        } else {
            this.SourceRectangle = sourceRectangle;
        }
        if ( color == default ) {
            this.Color = Color.White;
        } else {
            this.Color = color;
        }
        if ( effects == default ) {
            this.Effects = SpriteEffects.None;
        } else {
            this.Effects = effects;
        }
        if ( layerDepth == default ) {
            this.LayerDepth = 0;
        } else {
            this.LayerDepth = layerDepth;
        }
    }

    /// <summary>
    /// Draws the sprite
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to draw to.</param>
    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(this.Texture, this.Position, this.SourceRectangle, this.Color, this.Rotation, Vector2.Zero, this.Scale, this.Effects, this.LayerDepth);
    }

    /// <summary>
    /// Gets the color filter of the sprite.
    /// </summary>
    public Color Color { get; set;  }

    /// <summary>
    /// Gets the effects of the sprite.
    /// </summary>
    public SpriteEffects Effects { get; set; }

    /// <summary>
    /// Gets the layer depth of the sprite.
    /// </summary>
    public float LayerDepth { get; }

    /// <summary>
    /// Gets or sets the position of the sprite.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets the rotation of the sprite in radians.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// Gets the scale of the sprite.
    /// </summary>
    public float Scale { get; set; }

    /// <summary>
    /// Gets the source rectangle of the sprite.
    /// </summary>
    public Rectangle SourceRectangle { get; }

    /// <summary>
    /// Gets the texture of the sprite.
    /// </summary>
    private TextureBundle Textures { get; }

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

    public void SetTexture(TextureType type) {
        if (this.Textures.CurrentState == type) {
            return;
        }
        this.Textures.SetState(type);
    }
}

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam14.Game.Graphics;
internal class SpriteManager : IDisposable {
    public SpriteManager() {
        this._spriteBatch = Game2.Instance().SpriteBatch;
        this._effect = this.GetDefaultEffect();
        this._disposed = false;
    }
    private bool _disposed;

    public void Begin(Camera camera) {
        if ( camera == null ) {
            Viewport viewport = Game2.Instance().GraphicsDevice.Viewport;
            this._effect.Projection = Matrix.CreateOrthographicOffCenter(
                left: 0,
                right: viewport.Width,
                bottom: viewport.Height,
                top: 0,
                zNearPlane: 0f,
                zFarPlane: 1f
            );
            this._effect.View = Matrix.Identity;
        } else {
            camera.UpdateProjectionMatrix();
            camera.UpdateViewMatrix();
            this._effect.Projection = camera.Projection;
            this._effect.View = camera.View;
        }

        this._spriteBatch.Begin(
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            rasterizerState: RasterizerState.CullNone,
            effect: this._effect
        );
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Draw(Texture2D texture, Vector2 origin, Vector2 position, Color color) {
        Vector2 center = new Vector2(texture.Width / 2, texture.Height / 2);

        this._spriteBatch.Draw(
            texture: texture,
            position: position - center,
            sourceRectangle: null,
            color: color,
            rotation: 0f,
            origin: origin,
            scale: 1f,
            effects: SpriteEffects.None,
            layerDepth: 0f
        );
    }

    public void Draw(Entity.Entity entity) {
        // Debug.WriteLine("Drawing entity: " + entity);
        Vector2 center = new Vector2(entity.Sprite.Texture.Width / 2, entity.Sprite.Texture.Height / 2);
        this._spriteBatch.Draw(
            texture: entity.Sprite.Texture,
            position: entity.Position - center,
            sourceRectangle: entity.Sprite.SourceRectangle,
            color: entity.Sprite.Color,
            rotation: entity.Sprite.Rotation,
            origin: entity.Sprite.Origin,
            scale: entity.Sprite.Scale,
            effects: entity.Sprite.Effects,
            layerDepth: entity.Sprite.LayerDepth
        );
    }

    public void Draw(Texture2D texture, Rectangle sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color color) {
        this._spriteBatch.Draw(
            texture: texture,
            position: position,
            sourceRectangle: sourceRectangle,
            color: color,
            rotation: rotation,
            origin: origin,
            scale: scale,
            effects: SpriteEffects.None,
            layerDepth: 0f
        );
    }

    public void Draw(Texture2D texture, Rectangle sourceRectangle, Rectangle destinationRectangle, Color color) {
        this._spriteBatch.Draw(
            texture: texture,
            destinationRectangle: destinationRectangle,
            sourceRectangle: sourceRectangle,
            color: color
        );
    }

    public void End() {
        this._spriteBatch.End();
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this._effect.Dispose();
        this._spriteBatch.Dispose();
        this.Dispose();
        this._disposed = true;
    }

    private BasicEffect _effect;
    private SpriteBatch _spriteBatch;
    private BasicEffect GetDefaultEffect() {
        BasicEffect effect = new BasicEffect(Game2.Instance().GraphicsDevice);
        effect.VertexColorEnabled = true;
        effect.LightingEnabled = false;
        //effect.LightingEnabled = true;
        //effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
        //effect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
        //effect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 0);
        effect.FogEnabled = false;
        effect.TextureEnabled = true;
        effect.World = Matrix.Identity;
        effect.View = Matrix.Identity;
        effect.Projection = Matrix.Identity;
        return effect;
    }
}

using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam14.Game.Graphics;
internal class Screen : IDisposable {
    private static readonly int _min_width = 720;
    private static readonly int _min_height = 576;

    private static readonly int _max_width = 7680;
    private static readonly int _max_height = 4320;

    private readonly int _width;
    private readonly int _height;

    private bool _disposed;

    public int Width { get { return this._width; } }
    public int Height { get { return this._height; } }

    private readonly RenderTarget2D _renderTarget;

    public Screen(int width, int height) {
        if ( width < _min_width || width > _max_width ) {
            throw new ArgumentOutOfRangeException(nameof(width), width, $"Width must be between {_min_width} and {_max_width}");
        }
        if ( height < _min_height || height > _max_height ) {
            throw new ArgumentOutOfRangeException(nameof(height), height, $"Height must be between {_min_height} and {_max_height}");
        }

        this._width = width;
        this._height = height;
        this._renderTarget = new RenderTarget2D(Game2.Instance().GraphicsDevice, _width, _height);
        this._disposed = false;
    }

    public void Set() {
        if ( this.IsSet() ) {
            return;
        }
        Game2.Instance().GraphicsDevice.SetRenderTarget(this._renderTarget);
    }

    public void Unset() {
        if ( !this.IsSet() ) {
            return;
        }

        Game2.Instance().GraphicsDevice.SetRenderTarget(null);
    }

    public bool IsSet() {
        return Game2.Instance().GraphicsDevice.GetRenderTargets().Contains(this._renderTarget);
    }

    public void Present(SpriteBatch spriteBatch) {
#if DEBUG
        Game2.Instance().GraphicsDevice.Clear(Color.HotPink);
#else
        Game2.Instance().GraphicsDevice.Clear(Color.Black);
#endif

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
        spriteBatch.Draw(this._renderTarget, this.CalculateDestinationRectangle(), Color.White);
        spriteBatch.End();
    }

    private Rectangle CalculateDestinationRectangle() {
        Rectangle bounds = Game2.Instance().GraphicsDevice.PresentationParameters.Bounds;
        float aspectRatio = (float) bounds.Width / (float) bounds.Height;
        float targetAspectRatio = (float) _width / (float) _height;

        int targetX = 0;
        int targetY = 0;
        int targetWidth = bounds.Width;
        int targetHeight = bounds.Height;

        if ( aspectRatio > targetAspectRatio ) {
            targetWidth = (int) ( (float) bounds.Height * targetAspectRatio );
            targetX = (int) ( ( bounds.Width - targetWidth ) / 2f );
        } else if ( aspectRatio < targetAspectRatio ) {
            targetHeight = (int) ( (float) bounds.Width / targetAspectRatio );
            targetY = (int) ( ( bounds.Height - targetHeight ) / 2f );
        }
        return new Rectangle(targetX, targetY, targetWidth, targetHeight);
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this._renderTarget.Dispose();
        this.Dispose();
        this._disposed = true;
    }
}

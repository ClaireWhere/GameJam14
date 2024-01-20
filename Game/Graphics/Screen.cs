﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam14.Game.Graphics;
internal class Screen : IDisposable {
    private readonly static int _min_width = 720;
    private readonly static int _min_height = 576;

    private readonly static int _max_width = 7680;
    private readonly static int _max_height = 4320;

    private readonly int _width;
    private readonly int _height;

    private bool _isDisposed;
    private readonly RenderTarget2D _renderTarget;

    public Screen(int width, int height) {
        if (width < _min_width || width > _max_width) {
            throw new ArgumentOutOfRangeException("width", width, $"Width must be between {_min_width} and {_max_width}");
        }
        if (height < _min_height || height > _max_height) {
            throw new ArgumentOutOfRangeException("height", height, $"Height must be between {_min_height} and {_max_height}");
        }

        this._isDisposed = false;
        this._width = width;
        this._height = height;
        this._renderTarget = new RenderTarget2D(Game2.Instance().GraphicsDevice, _width, _height);
    }

    public void Dispose() {
        if (this._isDisposed) {
            return;
        }
        this._renderTarget.Dispose();
        this._isDisposed = true;
    }

    public void Set() {
        if (this.IsSet()) {
            return;
        }
        Game2.Instance().GraphicsDevice.SetRenderTarget(this._renderTarget);
    }

    public void Unset() {
        if (!this.IsSet()) {
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
        float aspectRatio = (float)bounds.Width / (float)bounds.Height;
        float targetAspectRatio = (float)_width / (float)_height;

        int targetX = 0;
        int targetY = 0;
        int targetWidth = bounds.Width;
        int targetHeight = bounds.Height;

        if (aspectRatio > targetAspectRatio) {
            targetWidth = (int)((float)bounds.Height * targetAspectRatio);
            targetX = (int)((bounds.Width - targetWidth) / 2f);
        } else if (aspectRatio < targetAspectRatio) {
            targetHeight = (int)((float)bounds.Width / targetAspectRatio);
            targetY = (int)((bounds.Height - targetHeight) / 2f);
        }
        return new Rectangle(targetX, targetY, targetWidth, targetHeight);
    }
}
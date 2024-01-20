using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameJam14.Game.Graphics;
internal class Camera
{
    public readonly static float minZoom = 0.1f;
    public readonly static float maxZoom = 2.0f;

    private Vector2 position { get; set; }
    private float zoom { get; set; }
    private float rotation { get; set; }
    private Vector2 origin { get; set; }
    private Rectangle bounds { get; set; }

    private Matrix view { get; set; }
    private Matrix projection { get; set; }


    public Camera()
    {
        zoom = 1.0f;
        rotation = 0.0f;
        position = Vector2.Zero;
        origin = Vector2.Zero;
        bounds = Rectangle.Empty;
    }

    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
            Matrix.CreateRotationZ(rotation) *
            Matrix.CreateScale(zoom) *
            Matrix.CreateTranslation(new Vector3(origin, 0));
    }

    public void Update(GameTime gameTime)
    {

    }
}

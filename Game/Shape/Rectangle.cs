using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Shape;
struct Rectangle {
    public Vector2 Source; //  Top left corner
    public float Width;
    public float Height;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> class.
    /// </summary>
    /// <param name="source">The source point vector.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Rectangle(Vector2 source, float width, float height) {
        Source = source;
        Width = width;
        Height = height;
    }

}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Shape;
struct Circle {
    public Vector2 Center;
    public float Radius;

    /// <summary>
    /// Initializes a new instance of the <see cref="Circle"/> class.
    /// </summary>
    /// <param name="center">The center.</param>
    /// <param name="radius">The radius.</param>
    public Circle(Vector2 center, float radius) {
        Center = center;
        Radius = radius;
    }
}

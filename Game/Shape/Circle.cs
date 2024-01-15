using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Shape;

/// <summary>
/// A Circle.
/// </summary>
public class Circle {

    /// <summary>
    /// Gets the center of the circle.
    /// </summary>
    public Vector2 Center { get; }

    /// <summary>
    /// Gets the radius of the circle.
    /// </summary>
    public float Radius { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Circle"/> class.
    /// </summary>
    /// <param name="center">The center.</param>
    /// <param name="radius">The radius.</param>
    public Circle(Vector2 center, float radius) {
        Center = center;
        Radius = radius;
    }

    /// <summary>
    /// Checks whether this circle contains the provided point
    /// </summary>
    /// <param name="point">The point to check whether this circle contains.</param>
    /// <returns>True if the point is within or on the bounds of the circle, False otherwise.</returns>
    public bool Contains(Vector2 point) {
        return Vector2.Distance(this.Center, point) <= Radius;
    }

    /// <summary>
    /// Checks whether this circle is intersecting with the provided circle.
    /// </summary>
    /// <param name="circle">The circle to check whether this circle contains.</param>
    /// <returns>True if the circle is within or on the bounds of the circle, False otherwise.</returns>
    public bool Intersects(Circle circle) {
        return Vector2.Distance(this.Center, circle.Center) <= ( this.Radius + circle.Radius );
    }


    /// <summary>
    /// Checks whether this circle is intersecting with the provided line.
    /// </summary>
    /// <param name="line">The line to check whether this circle contains.</param>
    /// <returns>True if the line is within or on the bounds of the circle, False otherwise.</returns>
    public bool Intersects(LineSegment line) {
        float closestX = (float) Math.Min(Math.Pow(this.Center.X - line.Start.X, 2), Math.Pow(this.Center.X - line.End.X, 2));
        float closestY = (float) Math.Min(Math.Pow(this.Center.Y - line.Start.Y, 2), Math.Pow(this.Center.Y - line.End.Y, 2));

        return Vector2.Distance(Center, new Vector2(closestX, closestY)) <= Radius;
    }

    /// <summary>
    /// Checks whether this circle is intersecting with the provided rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to check whether this circle contains.</param>
    /// <returns>True if the rectangle is within or on the bounds of the circle, False otherwise.</returns>
    public bool Intersects(Rectangle rect) {
        float checkX = this.Center.X;
        float checkY = this.Center.Y;

        if (this.Center.X < rect.Left) {
            checkX = rect.Left;
        }
        else if (this.Center.X > rect.Right) {
            checkX = rect.Right;
        }

        if (this.Center.Y < rect.Top) {
            checkY = rect.Top;
        }
        else if (this.Center.Y > rect.Bottom) {
            checkY = rect.Bottom;
        }

        return Vector2.Distance(Center, new Vector2(checkX, checkY)) <= Radius;



    }


}

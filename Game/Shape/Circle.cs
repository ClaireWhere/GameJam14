using Microsoft.Xna.Framework;

using System;

namespace GameJam14.Game.Shape;

/// <summary>
///   A Circle.
/// </summary>
public class Circle : Shape {
    /// <summary>
    ///   Initializes a new instance of the <see cref="Circle" /> class.
    /// </summary>
    /// <param name="center">
    ///   The center.
    /// </param>
    /// <param name="radius">
    ///   The radius.
    /// </param>
    public Circle(Vector2 center, float radius) {
        this.Position = center;
        this.Radius = radius;
        this.Scale = 1;
    }

    /// <summary>
    ///   Gets the radius of the circle.
    /// </summary>
    public float Radius {
        get {
            return this._radius * this.Scale;
        }
        set {
            this._radius = value;
        }
    }

    private float _radius;

    /// <summary>
    ///   Checks whether this circle contains the provided point
    /// </summary>
    /// <param name="point">
    ///   The point to check whether this circle contains.
    /// </param>
    /// <returns>
    ///   True if the point is within or on the bounds of the circle, False otherwise.
    /// </returns>
    public override bool Contains(Vector2 point) {
        return Vector2.Distance(this.Position, point) <= this.Radius;
    }

    public override bool Intersects(Shape shape) {
        return shape.Intersects(this);
    }

    /// <summary>
    ///   Checks whether this circle is intersecting with the provided circle.
    /// </summary>
    /// <param name="circle">
    ///   The circle to check whether this circle contains.
    /// </param>
    /// <returns>
    ///   True if the circle is within or on the bounds of the circle, False otherwise.
    /// </returns>
    public override bool Intersects(Circle circle) {
        return Vector2.Distance(this.Position, circle.Position) <= ( this.Radius + circle.Radius );
    }

    /// <summary>
    ///   Checks whether this circle is intersecting with the provided line.
    /// </summary>
    /// <param name="line">
    ///   The line to check whether this circle contains.
    /// </param>
    /// <returns>
    ///   True if the line is within or on the bounds of the circle, False otherwise.
    /// </returns>
    public override bool Intersects(LineSegment line) {
        float closestX = (float)Math.Min(Math.Pow(this.Position.X - line.Start.X, 2), Math.Pow(this.Position.X - line.End.X, 2));
        float closestY = (float)Math.Min(Math.Pow(this.Position.Y - line.Start.Y, 2), Math.Pow(this.Position.Y - line.End.Y, 2));

        return Vector2.Distance(this.Position, new Vector2(closestX, closestY)) <= this.Radius;
    }

    /// <summary>
    ///   Checks whether this circle is intersecting with the provided rectangle.
    /// </summary>
    /// <param name="rectangle">
    ///   The rectangle to check whether this circle contains.
    /// </param>
    /// <returns>
    ///   True if the rectangle is within or on the bounds of the circle, False otherwise.
    /// </returns>
    public override bool Intersects(Rectangle rectangle) {
        float checkX = this.Position.X;
        float checkY = this.Position.Y;

        if ( this.Position.X < rectangle.Left ) {
            checkX = rectangle.Left;
        } else if ( this.Position.X > rectangle.Right ) {
            checkX = rectangle.Right;
        }

        if ( this.Position.Y < rectangle.Top ) {
            checkY = rectangle.Top;
        } else if ( this.Position.Y > rectangle.Bottom ) {
            checkY = rectangle.Bottom;
        }

        return Vector2.Distance(this.Position, new Vector2(checkX, checkY)) <= this.Radius;
    }

    public override string ToString() {
        return "<Circle> -> " + base.ToString() + " | Center: " + this.Position.ToString() + " | Radius: " + this.Radius;
    }

}

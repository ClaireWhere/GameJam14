﻿using Microsoft.Xna.Framework;

namespace GameJam14.Game.Shape;

/// <summary>
///   A Rectangle.
/// </summary>
public class Rectangle : Shape {
    /// <summary>
    ///   Initializes a new instance of the <see cref="Rectangle" /> class.
    /// </summary>
    /// <param name="source">
    ///   The source point vector.
    /// </param>
    /// <param name="width">
    ///   The width.
    /// </param>
    /// <param name="height">
    ///   The height.
    /// </param>
    public Rectangle(Vector2 source, float width, float height) {
        this.Source = source;
        this.Width = width;
        this.Height = height;
    }

    /// <summary>
    ///   Gets the bottom (Y-coordinate) of the rectangle.
    /// </summary>
    public float Bottom {
        get {
            return this.ScaledSource.Y + this.ScaledHeight;
        }
    }

    /// <summary>
    ///   Gets the height of the rectangle.
    /// </summary>
    public float Height { get; }

    /// <summary>
    ///   Gets the left (X-coordinate) of the rectangle.
    /// </summary>
    public float Left {
        get {
            return this.ScaledSource.X;
        }
    }

    /// <summary>
    ///   Gets the right (X-coordinate) of the rectangle.
    /// </summary>
    public float Right {
        get {
            return this.ScaledSource.X + this.ScaledWidth;
        }
    }

    public float ScaledHeight {
        get {
            return this.Height * this.Scale;
        }
    }

    public Vector2 ScaledSource {
        get {
            float sourceX = this.Source.X - ( this.Width * this.Scale / 2 );
            float sourceY = this.Source.Y - ( this.Height * this.Scale / 2 );
            return new Vector2(sourceX, sourceY);
        }
    }

    public float ScaledWidth {
        get {
            return this.Width * this.Scale;
        }
    }

    /// <summary>
    ///   Top left corner
    /// </summary>
    public Vector2 Source { get; }

    /// <summary>
    ///   Gets the top (Y-coordinate) of the rectangle.
    /// </summary>
    public float Top {
        get {
            return this.ScaledSource.Y;
        }
    }

    /// <summary>
    ///   Gets the width of the rectangle.
    /// </summary>
    public float Width { get; }

    /// <summary>
    ///   Checks whether this rectangle contains the provided point
    /// </summary>
    /// <param name="point">
    ///   The point to check whether this rectangle contains.
    /// </param>
    /// <returns>
    ///   True if the point is within or on the bounds of the rectangle, False otherwise.
    /// </returns>
    public override bool Contains(Vector2 point) {
        return point.X >= this.Left && point.X <= this.Right && point.Y >= this.Top && point.Y <= this.Bottom;
    }

    public override bool Intersects(Shape shape) {
        return shape.Intersects(this);
    }

    /// <summary>
    ///   Checks whether this rectangle is intersecting with the provided line.
    /// </summary>
    /// <param name="line">
    ///   The line to check whether this rectangle contains.
    /// </param>
    /// <returns>
    ///   True if the line is within or on the bounds of the rectangle, False otherwise.
    /// </returns>
    public override bool Intersects(LineSegment line) {
        return line.Intersects(this);
    }

    /// <summary>
    ///   Checks whether this rectangle is intersecting with the provided rectangle.
    /// </summary>
    /// <param name="rectangle">
    ///   The rectangle to check whether this rectangle contains.
    /// </param>
    /// <returns>
    ///   True if the rectangle is within or on the bounds of the rectangle, False otherwise.
    /// </returns>
    /// <returns>
    ///   A bool.
    /// </returns>
    public override bool Intersects(Rectangle rectangle) {
        return this.Right >= rectangle.Left
            && this.Left <= rectangle.Right
            && this.Top >= rectangle.Bottom
            && this.Bottom <= rectangle.Top;
    }

    /// <summary>
    ///   Checks whether this rectangle is intersecting with the provided circle.
    /// </summary>
    /// <param name="circle">
    ///   The circle to check whether this rectangle contains.
    /// </param>
    /// <returns>
    ///   True if the circle is within or on the bounds of the rectangle, False otherwise.
    /// </returns>
    public override bool Intersects(Circle circle) {
        return circle.Intersects(this);
    }
}

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

    /// <summary>
    /// Gets the top (Y-coordinate) of the rectangle.
    /// </summary>
    public readonly float Top {
        get {
            return Source.Y;
        }
    }

    /// <summary>
    /// Gets the bottom (Y-coordinate) of the rectangle.
    /// </summary>
    public readonly float Bottom {
        get {
            return Source.Y + Height;
        }
    }

    /// <summary>
    /// Gets the left (X-coordinate) of the rectangle.
    /// </summary>
    public readonly float Left {
        get {
            return Source.X;
        }
    }

    /// <summary>
    /// Gets the right (X-coordinate) of the rectangle.
    /// </summary>
    public readonly float Right {
        get {
            return Source.X + Width;
        }
    }

    /// <summary>
    /// Checks whether this rectangle contains the provided point
    /// </summary>
    /// <param name="point">The point to check whether this rectangle contains.</param>
    /// <returns>True if the point is within or on the bounds of the rectangle, False otherwise.</returns>
    public readonly bool Contains(Vector2 point) {
        return point.X >= this.Left && point.X <= this.Right && point.Y >= this.Top && point.Y <= this.Bottom;
    }

    /// <summary>
    /// Checks whether this rectangle is intersecting with the provided line.
    /// </summary>
    /// <param name="line">The line to check whether this rectangle contains.</param>
    /// <returns>True if the line is within or on the bounds of the rectangle, False otherwise.</returns>
    public readonly bool Intersects(LineSegment line) {
        return line.Intersects(this);
    }

    /// <summary>
    /// Checks whether this rectangle is intersecting with the provided rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to check whether this rectangle contains.</param>
    /// <returns>True if the rectangle is within or on the bounds of the rectangle, False otherwise.</returns>
    /// <returns>A bool.</returns>
    public readonly bool Intersects(Rectangle rect) {
        return this.Right >= rect.Left
            && this.Left <= rect.Right
            && this.Top >= rect.Bottom
            && this.Bottom <= rect.Top;
    }

    /// <summary>
    /// Checks whether this rectangle is intersecting with the provided circle.
    /// </summary>
    /// <param name="circle">The circle to check whether this rectangle contains.</param>
    /// <returns>True if the circle is within or on the bounds of the rectangle, False otherwise.</returns>
    public readonly bool Intersects(Circle circle) {
        return circle.Intersects(this);
    }


}

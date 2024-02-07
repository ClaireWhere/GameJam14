using Microsoft.Xna.Framework;

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
    public Rectangle(Vector2 source, float width, float height, float scale = 1f) {
        this.Position = source;
        this.Width = width;
        this.Height = height;
        this.Scale = scale;
    }

    /// <summary>
    ///   Gets the bottom (Y-coordinate) of the rectangle.
    /// </summary>
    public float Bottom {
        get {
            return this.Position.Y + this.Height;
        }
    }

    /// <summary>
    ///   Gets the scaled height of the rectangle.
    /// </summary>
    public float Height {
        get {
            return this._height * this.Scale;
        }
        set {
            this._height = value;
        }
    }

    private float _height;

    /// <summary>
    ///   Gets the left (X-coordinate) of the rectangle.
    /// </summary>
    public float Left {
        get {
            return this.Position.X;
        }
    }

    /// <summary>
    ///   Gets the right (X-coordinate) of the rectangle.
    /// </summary>
    public float Right {
        get {
            return this.Position.X + this.Width;
        }
    }

    /// <summary>
    ///   Gets the top (Y-coordinate) of the rectangle.
    /// </summary>
    public float Top {
        get {
            return this.Position.Y;
        }
    }

    /// <summary>
    ///   Gets the scaled width of the rectangle.
    /// </summary>
    public float Width {
        get {
            return this._width * this.Scale;
        }
        set {
            this._width = value;
        }
    }
    private float _width;

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

    public override string ToString() {
        return "<Rectangle> -> " + base.ToString() + " | Width: " + this.Width + " | Height: " + this.Height;
    }
}

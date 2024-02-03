using Microsoft.Xna.Framework;

using System;

namespace GameJam14.Game.Shape;

/// <summary>
///   A line segment.
/// </summary>
public class LineSegment : Shape {
    /// <summary>
    ///   Initializes a new instance of the <see cref="LineSegment" /> class.
    /// </summary>
    /// <param name="start">
    ///   The start (source) vector of the line segment.
    /// </param>
    /// <param name="end">
    ///   The end (destination) vector of the line segment.
    /// </param>
    public LineSegment(Vector2 start, Vector2 end) {
        this.Start = start;
        this.End = end;
        this.Position = Vector2.Zero;
        this.Scale = 1;
    }

    /// <summary>
    ///   Gets the destination vector of the line segment.
    /// </summary>
    public Vector2 End {
        get {
            return ( this._end * this.Scale ) + this.Position;
        }
        set {
            this._end = value;
        }
    }

    private Vector2 _end;

    /// <summary>
    ///   Gets the length of the line segment.
    /// </summary>
    public float Length {
        get {
            return Vector2.Distance(this.Start, this.End);
        }
    }

    /// <summary>
    ///   Gets the source vector of the line segment.
    /// </summary>
    public Vector2 Start {
        get {
            return ( this._start * this.Scale ) + this.Position;
        }
        set {
            this._start = value;
        }
    }

    private Vector2 _start;

    /// <summary>
    ///   Checks whether specified point falls on the line segment.
    /// </summary>
    /// <param name="point">
    ///   The point to check whether it falls on the line segment.
    /// </param>
    /// <returns>
    ///   True if the point falls on the line segment, false otherwise.
    /// </returns>
    public override bool Contains(Vector2 point) {
        // If the point is on the line, it must be contained on the X axis
        if ( ( point.X < this.Start.X ) == ( point.X < this.End.X ) ) {
            return false;
        }
        // If the point is on the line, it must be contained on the Y axis
        if ( ( point.Y < this.Start.Y ) == ( point.Y < this.End.Y ) ) {
            return false;
        }

        // If the point is on the line, it must have the same slope to one end point as the line
        // segment itself. Here, we compare the slope from the point to the Start of the line
        // segment, but it could be End as well, since the line segment is straight.
        return ( this.Start.Y - point.Y ) * ( this.End.X - this.Start.X ) == ( this.End.Y - this.Start.Y ) * ( this.Start.X - point.X );
    }

    public Vector2 GetIntersection(LineSegment line) {
        // (x1, x2) = (this.Start.X, this.End.X) (y1, y2) = (this.Start.Y, this.End.Y) (x3, x4) =
        // (line.Start.X, line.End.X) (y3, y4) = (line.Start.Y, line.End.Y)

        // (x4-x3)(y3-y1) - (y4-y3)(x3-x1) (x4-x3)(y2-y1) - (y4-y3)(x2-x1)

        // (x2-x1)(y3-y1) - (y2-y1)(x3-x1) (x4-x3)(y2-y) - (y4-y3)(x2-x1)
        float denominator = ( ( line.End.X - line.Start.X ) * ( this.End.Y - this.Start.Y ) ) - ( ( line.End.Y - line.Start.Y ) * ( this.End.X - this.Start.X ) );

        float intersectThisNumerator = ( ( line.End.X - line.Start.X ) * ( line.Start.Y - this.Start.Y ) ) - ( ( line.Start.Y - line.End.Y ) * ( line.Start.X - this.Start.X ) );
        float intersectOtherNumerator = ( ( this.End.X - this.Start.X ) * ( line.Start.Y - this.Start.Y ) ) - ( ( this.End.Y - this.Start.Y ) * ( line.Start.X - this.Start.X ) );

        // if denominator is 0, the lines are parallel, so they do not intersect. If the numerator
        // is also 0, the lines are collinear (return the center).
        if ( denominator == 0 ) {
            return intersectThisNumerator == 0 && intersectOtherNumerator == 0
                ? new Vector2(this.Start.X + ( ( this.End.X - this.Start.X ) / 2 ), this.Start.Y + ( ( this.End.Y - this.Start.Y ) / 2 ))
                : Vector2.Zero;
        }

        // The distance along the "this" line at which the intersection occurs
        float intersectThis = intersectThisNumerator / denominator;
        // The distance along the "other" line at which the intersection occurs
        float intersectOther = intersectOtherNumerator / denominator;

        return intersectThis >= 0 && intersectThis <= 1 && intersectOther >= 0 && intersectOther <= 1
            ? new Vector2(this.Start.X + ( ( this.End.X - this.Start.X ) * intersectThis ), this.Start.Y + ( ( this.End.Y - this.Start.Y ) * intersectThis ))
            : Vector2.Zero;
    }

    public override bool Intersects(Shape shape) {
        return shape.Intersects(this);
    }

    /// <summary>
    ///   Checks whether the line segment intersects with the specified line segment.
    /// </summary>
    /// <param name="line">
    ///   The line segment to check intersection with.
    /// </param>
    /// <returns>
    ///   True if the line segments intersect or are collinear, false otherwise
    /// </returns>
    public override bool Intersects(LineSegment line) {
        float denominator = ( ( line.End.X - line.Start.X ) * ( this.End.Y - this.Start.Y ) ) - ( ( line.End.Y - line.Start.Y ) * ( this.End.X - this.Start.X ) );

        float intersectThisNumerator = ( ( line.End.X - line.Start.X ) * ( line.Start.Y - this.Start.Y ) ) - ( ( line.Start.Y - line.End.Y ) * ( line.Start.X - this.Start.X ) );

        return ( denominator < 0 ) == ( intersectThisNumerator < 0 ) && Math.Abs(denominator) >= Math.Abs(intersectThisNumerator);
    }

    /// <summary>
    ///   Checks whether the line segment intersects with the specified rectangle.
    /// </summary>
    /// <param name="rectangle">
    ///   The rectangle to check intersection with.
    /// </param>
    /// <returns>
    ///   True if the line segment intersects with the rectangle, false otherwise.
    /// </returns>
    public override bool Intersects(Rectangle rectangle) {
        bool left = this.Intersects(new LineSegment(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom)));
        bool right = this.Intersects(new LineSegment(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom)));
        bool top = this.Intersects(new LineSegment(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top)));
        bool bottom = this.Intersects(new LineSegment(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom)));

        return left || right || top || bottom;
    }

    /// <summary>
    ///   Checks whether the line segment intersects with the specified circle.
    /// </summary>
    /// <param name="circle">
    ///   The circle to check intersection with.
    /// </param>
    /// <returns>
    ///   True if the line segment intersects with the circle, false otherwise.
    /// </returns>
    public override bool Intersects(Circle circle) {
        return circle.Intersects(this);
    }
}

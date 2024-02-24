using Microsoft.Xna.Framework;

using System;

namespace GameJam14.Game.Shape;
/// <summary>
///   The base class for a shape.
/// </summary>
public abstract class Shape : IDisposable {
    public float Scale { get; set; } = 1f;

    /// <summary>
    ///   Determines whether this shape contains the specified point.
    /// </summary>
    /// <param name="point">
    ///   The point to check whether this shape contains.
    /// </param>
    /// <returns>
    ///   True if the point is within or on the bounds of the shape, False otherwise.
    /// </returns>
    public abstract bool Contains(Vector2 point);

    private bool _disposed;

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///   Determines whether this shape intersects the specified shape.
    /// </summary>
    /// <param name="shape">
    ///   The shape to check whether this shape intersects.
    /// </param>
    /// <returns>
    ///   True if the shape is within or on the bounds of the shape, False otherwise.
    /// </returns>
    public abstract bool Intersects(Shape shape);

    /// <summary>
    ///   Determines whether this shape intersects the specified rectangle.
    /// </summary>
    /// <param name="rectangle">
    ///   The rectangle to check whether this shape intersects.
    /// </param>
    /// <returns>
    ///   True if the rectangle is within or on the bounds of the shape, False otherwise.
    /// </returns>
    public abstract bool Intersects(Rectangle rectangle);

    /// <summary>
    ///   Determines whether this shape intersects the specified line segment.
    /// </summary>
    /// <param name="line">
    ///   The line segment to check whether this shape intersects.
    /// </param>
    /// <returns>
    ///   True if the line segment is within or on the bounds of the shape, False otherwise.
    /// </returns>
    public abstract bool Intersects(LineSegment line);

    /// <summary>
    ///   Determines whether this shape intersects the specified circle.
    /// </summary>
    /// <param name="circle">
    ///   The circle to check whether this shape intersects.
    /// </param>
    /// <returns>
    ///   True if the circle is within or on the bounds of the shape, False otherwise.
    /// </returns>
    public abstract bool Intersects(Circle circle);

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }

        this._disposed = true;
    }

    public override string ToString() {
        return "<Shape> -> Scale: " + this.Scale + " | Position: " + this.Position.ToString();
    }

    public abstract Shape Copy();

    public bool WillIntersect(Shape shape, Vector2 projectedPosition) {
        if ( this.Intersects(shape) ) {
            return true;
        }

        Shape projectedShape = this.Copy();
        projectedShape.Position = projectedPosition;
        // projectedShape.GetType().InvokeMember("Intersects", System.Reflection.BindingFlags.InvokeMethod, null, projectedShape, new object[] { shape });
        if ( projectedShape.Intersects(shape) ) {
            return true;
        }

        LineSegment projectedRay = new LineSegment(this.Position, projectedPosition);
        if ( projectedRay.Intersects(shape) ) {
            return true;
        }

        LineSegment inverseProjectedRay = new LineSegment(shape.Position, shape.Position - ( projectedPosition - this.Position ));
        if ( inverseProjectedRay.Intersects(this) ) {
            return true;
        }

        foreach ( Vector2 corner in this.Corners ) {
            LineSegment cornerRay = new LineSegment(this.Position + corner, projectedPosition + corner);
            if ( cornerRay.Intersects(shape) ) {
                return true;
            }
        }

        foreach ( Vector2 corner in shape.Corners ) {
            LineSegment cornerRay = new LineSegment(shape.Position + corner, this.Position + corner);
            if ( cornerRay.Intersects(this) ) {
                return true;
            }
        }

        return false;
    }
    public Vector2 Position { get; set; }
    public Vector2[] Corners { get; }
}

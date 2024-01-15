using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Shape;
/// <summary>
/// The base class for a shape.
/// </summary>
public abstract class Shape {
    public float Scale { get; set; } = 1f;

    /// <summary>
    /// Determines whether this shape contains the specified point.
    /// </summary>
    /// <param name="point">The point to check whether this shape contains.</param>
    /// <returns>True if the point is within or on the bounds of the shape, False otherwise.</returns>
    public abstract bool Contains(Vector2 point);
    /// <summary>
    /// Determines whether this shape intersects the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle to check whether this shape intersects.</param>
    /// <returns>True if the rectangle is within or on the bounds of the shape, False otherwise.</returns>
    public abstract bool Intersects(Rectangle rectangle);
    /// <summary>
    /// Determines whether this shape intersects the specified line segment.
    /// </summary>
    /// <param name="line">The line segment to check whether this shape intersects.</param>
    /// <returns>True if the line segment is within or on the bounds of the shape, False otherwise.</returns>
    public abstract bool Intersects(LineSegment line);
    /// <summary>
    /// Determines whether this shape intersects the specified circle.
    /// </summary>
    /// <param name="circle">The circle to check whether this shape intersects.</param>
    /// <returns>True if the circle is within or on the bounds of the shape, False otherwise.</returns>
    public abstract bool Intersects(Circle circle);
}

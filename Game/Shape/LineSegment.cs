using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Shape;
struct LineSegment {
    public Vector2 Start;
    public Vector2 End;

    /// <summary>
    /// Initializes a new instance of the <see cref="LineSegment"/> class.
    /// </summary>
    /// <param name="start">The start (source) vector of the line segment.</param>
    /// <param name="end">The end (destination) vector of the line segment.</param>
    public LineSegment(Vector2 start, Vector2 end) {
        Start = start;
        End = end;
    }

}

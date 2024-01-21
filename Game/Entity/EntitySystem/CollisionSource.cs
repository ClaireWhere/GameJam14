// Ignore Spelling: hitbox hitboxes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity.EntitySystem;
/// <summary>
/// Handles collisions between different types of objects and shapes.
/// </summary>
public class CollisionSource
{
    /// <summary>
    /// Gets or sets the type of the collision.
    /// </summary>
    public CollisionType Type { get; protected set; }

    /// <summary>
    /// A list of shapes that represent the area of collision of this object.
    /// </summary>
    public List<Shape.Shape> Hitbox { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CollisionSource"/> class.
    /// </summary>
    /// <param name="type">The type of the collision.</param>
    /// <param name="hitbox">The area of collision of the object.</param>
    public CollisionSource(CollisionType type, List<Shape.Shape> hitbox)
    {
        Type = type;
        Hitbox = hitbox;
    }

    /// <summary>
    /// Checks whether a collision can occur between this source and another.
    /// </summary>
    /// <param name="other">The other collision source.</param>
    /// <returns>True if the two sources are able to collide, false otherwise.</returns>
    public bool CanCollideWith(CollisionSource other)
    {
        return Type.Collides(other.Type);
    }

    /// <summary>
    /// Checks collision between this source and another.
    /// </summary>
    /// <param name="other">The other collision source.</param>
    /// <returns>True if the collision sources are able to collide and at least one collision occurs between the hitboxes of the collision sources, false otherwise.</returns>
    public bool CollidesWith(CollisionSource other)
    {
        if (!CanCollideWith(other))
        {
            return false;
        }

        for (int i = 0; i < Hitbox.Count; i++)
        {
            Shape.Shape shape = Hitbox[i];
            if (other.CollidesWith(shape))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks collision between the hitboxes of this source and a shape.
    /// </summary>
    /// <param name="hitbox">The shape of the hitbox to check collision with.</param>
    /// <returns>
    /// True if at least one collision occurs between the shape and this collision source's hitbox, false otherwise.
    /// </returns>
    public bool CollidesWith(Shape.Shape hitbox)
    {
        for (int i = 0; i < Hitbox.Count; i++)
        {
            Shape.Shape shape = Hitbox[i];
            if (shape.Intersects(hitbox))
            {
                return true;
            }
        }

        return false;
    }
}

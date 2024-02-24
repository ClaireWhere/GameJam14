using GameJam14.Game.Entity.EntitySystem;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace GameJam14.Game.Entity;
internal class Wall {
    private Vector2 _position;
    private readonly CollisionSource _collision;
    public Texture2D Texture { get; set; }

    public enum WallType {
        Rectangle,
        Circle
    }

    public Wall(Vector2 position, float width, float height) {
        this._position = position;
        this._collision = new CollisionSource(
            type: new CollisionType(
                solidType: CollisionType.SolidType.Wall,
                lightType: CollisionType.LightType.None
            ),
            collisionEffect: CollisionSource.CollisionEffect.PreventMovement,
            hitbox: new HitBox(
                shape: new Shape.Rectangle(
                    source: Vector2.Zero,
                    width: width,
                    height: height
                ), offset: new Vector2(width / 2, height / 2)
            )
        );
    }

    public Wall(Vector2 position, float radius) {
        this._position = position;
        this._collision = new CollisionSource(
            type: new CollisionType(
                solidType: CollisionType.SolidType.Wall,
                lightType: CollisionType.LightType.None
            ),
            collisionEffect: CollisionSource.CollisionEffect.PreventMovement,
            hitbox: new HitBox(
                shape: new Shape.Circle(
                    center: position,
                    radius: radius
                )
            )
        );
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(this.Texture, this._position, Color.White);
    }

    public bool CheckCollision(Entity entity) {
        return this._collision.CollidesWith(entity.Collision);
    }

    private float GetMaxSize() {
        Shape.Shape shape = this._collision.Hitboxes[0].Shape;
        return shape is Shape.Rectangle rectangle
            ? Math.Max(rectangle.Width, rectangle.Height)
            : shape is Shape.Circle circle ? circle.Radius : throw new NotSupportedException("Unknown hitbox shape");
    }

    public bool WillCollide(Entity entity, Vector2 projectedPosition) {
        // Check whether the entity is already inside the wall
        //   -> if inside of the wall already, don't bother checking for further collision.
        // Important that this is the first check, because the "moving towards wall" check
        //   may fail if the entity is already inside the wall
        if ( this._collision.CollidesWith(entity.Collision) ) {
            return true;
        }

        // Check that the entity is moving towards the wall
        if ( Vector2.Dot(projectedPosition - entity.Position, this._position - entity.Position) < 0 ) {
            return false;
        }

        // Check distance between wall and projected position based on size of wall to avoid unnecessary collision checks (i.e., if the wall is far away, don't bother checking for collision)
        if ( Vector2.Distance(projectedPosition, this._position) > this.GetMaxSize() ) {
            return false;
        }

        // Check basic collision (center of entity to projected position) - this is the most common case
        Shape.LineSegment ray = new Shape.LineSegment(entity.Position, projectedPosition);
        if ( this._collision.CollidesWith(ray) ) {
            return true;
        }

        // TODO: Add circle collision check

        // If the basic collision check fails, check the corners of the entities hitboxes projected to the projected position (offset by the hitbox corners)
        foreach ( HitBox hitbox in entity.Collision.Hitboxes ) {
            foreach ( Vector2 corner in hitbox.Corners ) {
                ray = new Shape.LineSegment(entity.Position + corner, projectedPosition + corner);
                if ( this._collision.CollidesWith(ray) ) {
                    return true;
                }
            }
        }

        return false;
    }
}

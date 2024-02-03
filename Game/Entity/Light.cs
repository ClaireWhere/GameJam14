using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace GameJam14.Game.Entity;
internal class Light : Entity {
    public Light(int id, Vector2 position, Sprite sprite, float size)
            : base(
                id: id,
                position: position,
                collision: new CollisionSource(
                    type: new CollisionType(
                        solidType: CollisionType.SolidType.NonSolid,
                        lightType: CollisionType.LightType.Light,
                        entityType: CollisionType.EntityType.Other,
                        playerCollision: false,
                        enemyCollision: false
                    ),
                    collisionEffect: CollisionSource.CollisionEffect.None,
                    hitbox: new HitBox(new Shape.Circle(position, 1))
                ),
                sprite: sprite
            ) {
        this.Size = size;
        this.Sprite.Scale = size / this.Sprite.Texture.Width;
        this.Collision.Hitboxes[0].Shape.Scale = size;
    }

    public float Size { get; set; }
}

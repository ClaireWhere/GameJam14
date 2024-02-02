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
                    hitbox: new List<Shape.Shape>() { new Shape.Circle(center: position, radius: 1) }),
                sprite: sprite
            ) {
        this.Size = size;
        this.Sprite.Scale = size / this.Sprite.Texture.Width;
        this.Collision.Hitbox[0].Scale = size;
    }

    public float Size { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace GameJam14.Game.Entity;
internal class Light : Entity {

    public float Size { get; set; }

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
                        enemyCollision:  false
                    ),
                    hitbox: new List<Shape.Shape>() { new Shape.Circle(center: position, radius: 1) }),
                sprite: sprite
            ) {
        this.Size = size;
        this.Sprite.Scale = size / this.Sprite.Texture.Width;
        this.Collision.Hitbox[0].Scale = size;
    }
}

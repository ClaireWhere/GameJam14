using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;
internal class Cloud : Entity {
    public float ExpansionSpeed { get; set; }
    public float ExpansionAcceleration { get; set; }
    public float MaxExpansion { get; set; }
    public int Damage { get; set; }
    public double DamageDegredation { get; set; }

    public float Expansion { get; set; }
    public float HitboxRadius { get; set; }

    // int id, Vector2 position, Vector2 velocity, Vector2 acceleration, CollisionSource collision, Sprite sprite
    public Cloud(int id, Vector2 position, Sprite sprite, float expansionSpeed, float expansionAcceleration, float maxExpansion, int damage, double damageDegradation)
            : base(
                id: id,
                position: position,
                velocity: Vector2.Zero,
                acceleration: Vector2.Zero,
                collision: new CollisionSource(
                    type: new CollisionType(CollisionType.SolidType.NonSolid,
                        CollisionType.LightType.None,
                        CollisionType.EntityType.Other, false, false),
                    hitbox: new List<Shape.Shape>() { new Shape.Circle(position, sprite.Texture.Width) }),
                sprite: sprite
            ) {
        this.ExpansionSpeed = expansionSpeed;
        this.ExpansionAcceleration = expansionAcceleration;
        this.Expansion = 0f;
        this.MaxExpansion = maxExpansion;
        this.Damage = damage;
        this.DamageDegredation = damageDegradation;
        this.HitboxRadius = sprite.Texture.Width;
    }
}

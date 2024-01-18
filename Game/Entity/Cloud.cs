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

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        this.UpdateExpansion(gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void UpdateExpansion(double deltaTime) {
        if ( this.Expansion < this.MaxExpansion ) {
            this.Expansion += ( this.ExpansionSpeed * (float) deltaTime ) + ( this.ExpansionAcceleration * (float) Math.Pow(deltaTime, 2) / 2 );
            this.ExpansionSpeed += this.ExpansionAcceleration * (float) deltaTime;

            // Update hit box and sprite based on expansion
            this.Collision.Hitbox[0].Scale = this.Expansion;
            this.Sprite.Scale = this.Expansion;
        }
        if ( this.Expansion >= this.MaxExpansion ) {
            this.Kill();
        }
    }
}

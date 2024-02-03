// Ignore Spelling: Hitbox

using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameJam14.Game.Entity;
internal class Cloud : Entity {
    // int id, Vector2 position, Vector2 velocity, Vector2 acceleration, CollisionSource collision,
    // Sprite sprite
    public Cloud(int id, Vector2 position, Sprite sprite, CollisionSource.CollisionEffect collisionEffect, float expansionSpeed, float expansionAcceleration, float maxExpansion, int damage, double damageDegradation)
            : base(
                id: id,
                position: position,
                collision: new CollisionSource(
                    type: new CollisionType(
                        solidType: CollisionType.SolidType.NonSolid,
                        lightType: CollisionType.LightType.None,
                        entityType: CollisionType.EntityType.Other, false, false),
                    collisionEffect: collisionEffect,
                    hitbox: new HitBox(new Shape.Circle(position, sprite.Texture.Width * sprite.Scale))
                ),
                sprite: sprite
            ) {
        this.ExpansionSpeed = expansionSpeed;
        this.ExpansionAcceleration = expansionAcceleration;
        this.Expansion = 0f;
        this.MaxExpansion = maxExpansion;
        this.Damage = damage;
        this.DamageDegradation = damageDegradation;
        this.HitboxRadius = sprite.Texture.Width;
    }

    public Cloud(int id, Vector2 position, Sprite sprite, List<CollisionSource.CollisionEffect> collisionEffects, float expansionSpeed, float expansionAcceleration, float maxExpansion, int damage, double damageDegradation)
            : base(
                id: id,
                position: position,
                collision: new CollisionSource(
                    type: new CollisionType(
                        solidType: CollisionType.SolidType.NonSolid,
                        lightType: CollisionType.LightType.None,
                        entityType: CollisionType.EntityType.Other, false, false),
                    collisionEffects: collisionEffects,
                    hitbox: new HitBox(new Shape.Circle(position, sprite.Texture.Width * sprite.Scale))
                ),
                sprite: sprite
            ) {
        this.ExpansionSpeed = expansionSpeed;
        this.ExpansionAcceleration = expansionAcceleration;
        this.Expansion = 0f;
        this.MaxExpansion = maxExpansion;
        this.Damage = damage;
        this.DamageDegradation = damageDegradation;
        this.HitboxRadius = sprite.Texture.Width;
    }

    public int Damage { get; set; }
    public double DamageDegradation { get; set; }
    public float Expansion { get; set; }
    public float ExpansionAcceleration { get; set; }
    public float ExpansionSpeed { get; set; }
    public float HitboxRadius { get; set; }
    public float MaxExpansion { get; set; }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        this.UpdateExpansion(gameTime.ElapsedGameTime.TotalSeconds);
    }

    public override void HandleCollision(Light light) {
        Debug.WriteLine("\tCloud -> Light Collision...");
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.DestroyLight) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") killed entity (" + light.GetType().Name + ")");
            light.Kill();
        } else {
            base.HandleCollision(light);
        }
    }
    public override void HandleCollision(EntityActor actor) {
        Debug.WriteLine("\tCloud -> EntityActor Collision...");
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Damage) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") killed entity (" + actor.GetType().Name + ")");
            actor.TakeDamage((int)Math.Floor(this.Damage * this.DamageDegradation));
        }
    }

    public void UpdateExpansion(double deltaTime) {
        if ( this.Expansion < this.MaxExpansion ) {
            this.Expansion += ( this.ExpansionSpeed * (float)deltaTime ) + ( this.ExpansionAcceleration * (float)Math.Pow(deltaTime, 2) / 2 );
            this.ExpansionSpeed += this.ExpansionAcceleration * (float)deltaTime;

            // Update hit box and sprite based on expansion
            this.Collision.Hitboxes[0].Shape.Scale = this.Expansion;
            this.Sprite.Scale = this.Expansion;
        }
        if ( this.Expansion >= this.MaxExpansion ) {
            this.Kill();
        }
    }
}

using System.Collections.Generic;

using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

namespace GameJam14.Game.Entity;
internal class Projectile : Entity {
    public Projectile(int id, Vector2 position, float speed, Vector2 angle, Sprite sprite, bool hitsPlayer, bool hitsEnemy, int power, double timeToLive, DeathEffect deathEffect)
            : base(id,
                position,
                new CollisionSource(
                    type: new CollisionType(CollisionType.SolidType.NonSolid, CollisionType.LightType.None, CollisionType.EntityType.Other, hitsPlayer, hitsEnemy),
                    collisionEffect: CollisionSource.CollisionEffect.Damage,
                    hitbox: new List<Shape.Shape>() { new Shape.Rectangle(new Vector2(0, 0), sprite.Texture.Width, sprite.Texture.Height) }
                ),
                sprite
            ) {
        this.Power = power;
        this.TimeToLive = timeToLive;
        this.TimeAlive = 0;
        this.Death = deathEffect;
        this.DirectedMove(angle, speed);
        this.Health = 1;
    }

    public Projectile(int id, Vector2 position, float speed, Vector2 angle, Sprite sprite, bool hitsPlayer, bool hitsEnemy, int power, double timeToLive, DeathEffect deathEffect, float slowAmount, float slowDuration)
        : base(
            id,
            position,
            new CollisionSource(new CollisionType(CollisionType.SolidType.NonSolid, CollisionType.LightType.None, CollisionType.EntityType.Other, hitsPlayer, hitsEnemy), CollisionSource.CollisionEffect.Damage, new List<Shape.Shape>() { new Shape.Rectangle(position, sprite.Texture.Width, sprite.Texture.Height) }), sprite) {
        this.Power = power;
        this.TimeToLive = timeToLive;
        this.TimeAlive = 0;
        this.Death = deathEffect;
        this.DirectedMove(angle, speed);
        this.StunTime = 0;
        this.SlowAmount = slowAmount;
        this.SlowTime = slowDuration;
        this.Health = 1;
    }

    /// <summary>
    ///   The death effect when this projectile dies. Default : The projectile will disappear.
    ///   Explode : The projectile will explode in a cloud effect. None : The projectile will never die.
    /// </summary>
    public enum DeathEffect {
        Default,
        Explode,
        None
    }

    public DeathEffect Death { get; set; }
    public int Power { get; set; }
    public int Health { get; set; }
    public double TimeAlive { get; set; }
    public double TimeToLive { get; set; }
    public bool IsAlive { get { return this.Health > 0; } }
    public override void Kill() {
        switch ( this.Death ) {
        case DeathEffect.None:
            break;
        case DeathEffect.Default:
            base.Kill();
            break;
        case DeathEffect.Explode:
            // create a new Cloud attack entity at this position
            base.Kill();
            break;
        }
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        this.UpdateTimeAlive(gameTime.ElapsedGameTime.TotalSeconds);
        if (!this.IsAlive) {
            Debug.WriteLine(this.GetType().Name + " Entity is not alive");
            this.Kill();
        }
    }

    public void UpdateTimeAlive(double deltaTime) {
        if ( this.TimeAlive != -1f && this.TimeAlive < this.TimeToLive ) {
            this.TimeAlive += deltaTime;
        }
        if ( this.TimeAlive >= this.TimeToLive ) {
            this.Kill();
            this.TimeAlive = -1f;
        }
    }

    public void TakeDamage(int damage) {
        Debug.WriteLine(this.GetType().Name + " Took " + damage + " damage");
        this.Health -= damage;
        if ( this.Health < 0 ) {
            this.Health = 0;
        }
    }
}

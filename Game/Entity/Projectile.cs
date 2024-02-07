using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System.Diagnostics;

namespace GameJam14.Game.Entity;
internal class Projectile : Entity {
    public Projectile(int id, Vector2 position, float speed, Vector2 angle, Sprite sprite, CollisionType.EntityType entityType, bool hitsPlayer, bool hitsEnemy, int power, double timeToLive, DeathEffect deathEffect)
            : base(id,
                position,
                collision: new CollisionSource(
                    type: new CollisionType(
                        solidType: CollisionType.SolidType.NonSolid,
                        lightType: CollisionType.LightType.None,
                        entityType: entityType,
                        playerCollision: hitsPlayer,
                        enemyCollision: hitsEnemy
                    ),
                    collisionEffect: CollisionSource.CollisionEffect.Damage,
                    hitbox: new HitBox(new Shape.Circle(Vector2.Zero, sprite.Texture.Width * sprite.Scale))
                ),
                sprite
            ) {
        this.Power = power;
        this.TimeToLive = timeToLive;
        this.TimeAlive = 0;
        this.Death = deathEffect;
        this.DirectedMove(angle, speed);
        this.StunTime = 0;
        this.SlowAmount = 1f;
        this.SlowTime = 0;
        this.Health = 1;
    }

    public Projectile(int id, Vector2 position, float speed, Vector2 angle, Sprite sprite, CollisionType.EntityType entityType, bool hitsPlayer, bool hitsEnemy, int power, double timeToLive, DeathEffect deathEffect, float slowAmount, float slowDuration)
        : base(
            id,
            position,
            collision: new CollisionSource(
                type: new CollisionType(
                    solidType: CollisionType.SolidType.NonSolid,
                    lightType: CollisionType.LightType.None,
                    entityType: entityType,
                    playerCollision: hitsPlayer,
                    enemyCollision: hitsEnemy
                ),
                collisionEffect: CollisionSource.CollisionEffect.Damage,
                hitbox: new HitBox(new Shape.Circle(Vector2.Zero, sprite.Texture.Width, sprite.Scale))
            ),
            sprite
        ) {
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

    public override void HandleCollision(Projectile projectile) {
        Debug.WriteLine("\tProjectile -> Projectile Collision...");
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Damage) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") damaged entity (" + projectile.GetType().Name + ")");
            projectile.TakeDamage(this.Power);
        } else {
            base.HandleCollision(projectile);
        }
    }

    public override void HandleCollision(Light light) {
        Debug.WriteLine("\tProjectile -> Light Collision...");
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.DestroyLight) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") killed entity (" + light.GetType().Name + ")");
            light.Kill();
        }
        base.HandleCollision(light);
    }

    public override void HandleCollision(EntityActor actor) {
        Debug.WriteLine("\tProjectile -> EntityActor Collision...");
        Debug.WriteLine("Entity (" + this.GetType().Name + ") collided with entity (" + actor.GetType().Name + ")");
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Damage) ) {
            Debug.WriteLine("Entity (" + actor.GetType().Name + ") damaged entity (" + actor.GetType().Name + ")");
            Debug.WriteLine("Entity (" + this.GetType().Name + ") was killed by colliding with entity (" + actor.GetType().Name + ")");
            actor.TakeDamage(this.Power);
            this.Kill();
        }
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Slow) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") slowed entity (" + actor.GetType().Name + ")");
            actor.Slow(this.SlowTime, this.SlowAmount);
        }
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Stun) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") stunned entity (" + actor.GetType().Name + ")");
            actor.Stun(this.StunTime);
        }
        base.HandleCollision(actor);
    }

    public DeathEffect Death { get; set; }
    public int Power { get; set; }
    public int Health { get; set; }
    public double TimeAlive { get; set; }
    public double TimeToLive { get; set; }
    public bool IsAlive { get { return this.Health > 0; } }
    public float SlowAmount { get; set; }
    public float SlowTime { get; set; }
    public float StunTime { get; set; }

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
        if ( !this.IsAlive ) {
            Debug.WriteLine(this.GetType().Name + " Entity is not alive");
            this.Kill();
        }
    }

    public void UpdateTimeAlive(double deltaTime) {
        if ( this.TimeAlive != -1f && this.TimeAlive < this.TimeToLive ) {
            this.TimeAlive += deltaTime;
        }
        if ( this.TimeAlive >= this.TimeToLive ) {
            Debug.WriteLine(this.GetType().Name + " Entity has expired after " + this.TimeAlive + "s of " + this.TimeToLive + "s");
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

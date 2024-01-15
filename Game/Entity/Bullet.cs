using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;
internal class Bullet : Entity {
    public int Power { get; set; }
    public double TimeToLive { get; set; }
    public double TimeAlive { get; set; }
    /// <summary>
    /// The death effect when this bullet dies.
    /// Default : The bullet will disappear.
    /// Explode : The bullet will explode in a cloud effect.
    /// None    : The bullet will never die.
    /// </summary>
    public enum DeathEffect {
        Default,
        Explode,
        None
    }
    public DeathEffect Death { get; set; }

    public Bullet(int id, Vector2 position, Vector2 velocity, Vector2 acceleration, Sprite sprite, bool hitsPlayer, bool hitsEnemy, int power, double timeToLive, DeathEffect deathEffect)
            : base(id,
                position,
                velocity,
                acceleration,
                new CollisionSource(
                    type: new CollisionType(CollisionType.SolidType.NonSolid, CollisionType.LightType.None, CollisionType.EntityType.Other, hitsPlayer, hitsEnemy),
                    hitbox: new List<Shape.Shape>() { new Shape.Rectangle(position, sprite.Texture.Width, sprite.Texture.Height) }
                ),
                sprite
            ) {
        this.Power = power;
        this.TimeToLive = timeToLive;
        this.TimeAlive = 0;
        this.Death = deathEffect;
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        this.UpdateTimeAlive(gameTime.ElapsedGameTime.TotalSeconds);
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
}
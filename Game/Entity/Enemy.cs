// Ignore Spelling: hitbox

using System;
using System.Collections.Generic;

using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using static GameJam14.Game.Entity.EntitySystem.Attack;

namespace GameJam14.Game.Entity;
internal class Enemy : EntityActor {
    public Enemy(int id, string name, Vector2 position, List<Shape.Shape> hitbox, Sprite sprite, Stats baseStats, Inventory inventory, Attack attack, Target target)
        : base(
            id: id,
            name: name,
            position: position,
            collision:
                new CollisionSource(
                    type: new CollisionType(CollisionType.SolidType.Solid, CollisionType.LightType.None, CollisionType.EntityType.Enemy, true, false),
                    collisionEffect: CollisionSource.CollisionEffect.Damage,
                    hitbox: hitbox
                ),
            sprite: sprite,
            baseStats: baseStats,
            inventory: inventory,
            attack: attack
        ) {
        this.Target = target;
    }

    public Entity CurrentTarget { get; set; }
    public Target Target { get; set; }
    public bool IsTargetInDistance() {
        if ( this.CurrentTarget == null ) {
            return false;
        }
        return this.Target.TargetDistance >= Vector2.Distance(this.CurrentTarget.Position, this.Position);
    }

    public bool IsTargetInRange() {
        if ( this.CurrentTarget == null ) {
            return false;
        }
        return this.Target.TargetRange >= Vector2.Distance(this.CurrentTarget.Position, this.Position);
    }

    public void MoveRandomIdle() {
        if ( this.IsMoving ) {
            return;
        }
        int xMagnitude = this._random.Next(-1, 2);
        int yMagnitude = this._random.Next(-1, 2);
        float distance = this._random.Next(5, 10);
        if ( xMagnitude == 0 && yMagnitude == 0 ) {
            return;
        }
        this.DestinationMove(
            destination: new Vector2(this.Position.X + ( xMagnitude * distance ), this.Position.Y + ( yMagnitude * distance )),
            speed: this.Stats.IdleSpeed
        );
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        if ( this.CurrentTarget != null ) {
            this.DestinationMove(this.CurrentTarget.Position, this.Stats.RunSpeed);
        } else {
            this.MoveRandomIdle();
        }

        if ( this.IsTargetInDistance() && this.Attack.CanAttack() ) {
            this.UseAttack();
        }
    }

    protected override void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this.CurrentTarget.Dispose();
        base.Dispose(disposing);
    }

    private readonly Random _random = new Random();
    private void Shoot() {
        Vector2 angle = new Vector2(this.CurrentTarget.Position.X - this.Position.X, this.CurrentTarget.Position.Y - this.Position.Y);
        angle.Normalize();

        Projectile projectile = new Projectile(
            id: 0,
            position: this.Position,
            speed: 100,
            angle: angle,
            sprite: Data.SpriteData.ProjectileSprite,
            hitsPlayer: true,
            hitsEnemy: false,
            power: this.Attack.AttackDamage,
            timeToLive: 5.0,
            deathEffect: Projectile.DeathEffect.Default
        );
        Game2.Instance().AddEntity(projectile);
    }

    private void Squish() {
        Cloud cloud = new Cloud(
            id: 0,
            position: this.Position,
            sprite: Data.SpriteData.CloudSprite,
			collisionEffect: CollisionSource.CollisionEffect.DestroyLight,
            expansionSpeed: 10,
            expansionAcceleration: 0,
            maxExpansion: 100,
            damage: this.Attack.AttackDamage,
            damageDegradation: 0.0
        );
        Game2.Instance().AddEntity(cloud);
    }

    private void UseAttack() {
        this.Attack.StartAttack();
        if ( this.Attack.Type == AttackType.Shoot ) {
            this.Shoot();
        } else if ( this.Attack.Type == AttackType.Squish ) {
            this.Squish();
        }
    }
}

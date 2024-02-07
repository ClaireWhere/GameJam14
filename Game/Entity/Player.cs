using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.GameSystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Diagnostics;

namespace GameJam14.Game.Entity;

internal class Player : EntityActor {
    public static Player Instance {
        get {
            return s_Instance ??= new Player();
        }
    }

    public static void UpdateInstance(string name, Vector2 position, TextureType currentTexture, Stats baseStats, Inventory inventory, int health) {
        s_Instance.UpdateInstance(name, baseStats, inventory);
        s_Instance.TeleportTo(position);

        s_Instance.SetHealth(health);
        s_Instance.Sprite.SetTexture(currentTexture);
    }

    public override void Update(GameTime gameTime) {
        this.UpdateVelocity();
        this.UpdateTexture();
        this.UpdateAttack();
        base.Update(gameTime);
    }

    public void UpdateTexture() {
        if ( Input.IsKeyDown(Keys.W) ) {
            this.Sprite.SetTexture(TextureType.FaceBack);
        } else if ( Input.IsKeyDown(Keys.A) ) {
            this.Sprite.SetTexture(TextureType.FaceLeft);
        } else if ( Input.IsKeyDown(Keys.S) ) {
            this.Sprite.SetTexture(TextureType.FaceFront);
        } else if ( Input.IsKeyDown(Keys.D) ) {
            this.Sprite.SetTexture(TextureType.FaceRight);
        }
    }

    public void UpdateVelocity() {
        Vector2 angle = Vector2.Zero;

        if ( Input.IsKeyDown(Keys.W) ) {
            angle.Y--;
        }
        if ( Input.IsKeyDown(Keys.A) ) {
            angle.X--;
        }
        if ( Input.IsKeyDown(Keys.S) ) {
            angle.Y++;
        }
        if ( Input.IsKeyDown(Keys.D) ) {
            angle.X++;
        }

        if ( angle.LengthSquared() > 0 ) {
            this.DirectedMove(angle, this.Stats.RunSpeed);
        } else {
            this.StopMoving();
        }
    }

    private static Player s_Instance;
    private Player() : base(
        id: 0,
        name: "Chok",
        position: Vector2.Zero,
        collision:
            new CollisionSource(
                type: new CollisionType(
                    solidType: CollisionType.SolidType.Solid,
                    lightType: CollisionType.LightType.None,
                    entityType: CollisionType.EntityType.Player,
                    playerCollision: false,
                    enemyCollision: false
                ),
                collisionEffect: CollisionSource.CollisionEffect.None,
                hitbox: new HitBox(
                    shape: new Shape.Rectangle(
                        source: Vector2.Zero,
                        width: Data.SpriteData.PlayerSprite.Texture.Width * Data.SpriteData.PlayerSprite.Scale,
                        height: Data.SpriteData.PlayerSprite.Texture.Height * Data.SpriteData.PlayerSprite.Scale
                    ),
                    offset: new Vector2(
                        x: Data.SpriteData.PlayerSprite.Texture.Width * Data.SpriteData.PlayerSprite.Scale / 2,
                        y: Data.SpriteData.PlayerSprite.Texture.Height * Data.SpriteData.PlayerSprite.Scale / 2
                    )
                )
            ),
        sprite: Data.SpriteData.PlayerSprite,
        baseStats: Data.StatData.PlayerStats,
        inventory: new Inventory(),
        attack: new Attack(
            attackRange: 0,
            attackDistance: 0,
            attackSpeed: 0,
            attackCooldown: 1.0,
            attackDamage: 10
        )
    ) {
    }

    private void Shoot() {
        Vector2 backportCenter = new Vector2(Game2.Instance().Graphics.PreferredBackBufferWidth / 2, Game2.Instance().Graphics.PreferredBackBufferHeight / 2);
        Vector2 center = this.Position - Game2.Instance().Camera.Position + backportCenter;
        Vector2 angle = Input.MousePosition - center;
        angle.Normalize();

        Projectile projectile = new Projectile(
            id: 0,
            position: this.Position,
            speed: 1000,
            angle: angle,
            sprite: Data.SpriteData.ProjectileSprite,
            entityType: CollisionType.EntityType.Other,
            hitsPlayer: false,
            hitsEnemy: true,
            power: this.Attack.AttackDamage,
            timeToLive: 0.5,
            deathEffect: Projectile.DeathEffect.Default
        );
        Debug.WriteLine("Projectile angle: " + angle);
        Game2.Instance().AddEntity(projectile);
    }

    private void UpdateAttack() {
        if ( Input.CheckMouseButtonState(Input.MouseButtonType.Left, ButtonState.Pressed) ) {
            Debug.WriteLine("Attack");
            if ( this.Attack.CanAttack() ) {
                Debug.WriteLine("Starting attack");
                this.Attack.StartAttack();
                this.Shoot();
            }
        }
    }
}

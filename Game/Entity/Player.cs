using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameJam14.Game.Entity;


internal class Player : EntityActor {
    private static Player s_Instance;
    public static Player Instance {
        get {
            return s_Instance ??= new Player();
        }
    }

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
                hitbox: new List<Shape.Shape>() { new Shape.Rectangle(Vector2.Zero, Data.SpriteData.PlayerSprite.Texture.Width, Data.SpriteData.PlayerSprite.Texture.Height) }
            ),
        sprite: Data.SpriteData.PlayerSprite,
        baseStats: Data.StatData.PlayerStats,
        inventory: new Inventory(),
        attack: new Attack()
    ) {

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
        base.Update(gameTime);
    }

    public void UpdateTexture() {
        if (Input.IsKeyDown(Keys.W)) {
            this.Sprite.SetTexture(TextureType.FaceBack);
        } else if (Input.IsKeyDown(Keys.A)) {
            this.Sprite.SetTexture(TextureType.FaceLeft);
        } else if (Input.IsKeyDown(Keys.S)) {
            this.Sprite.SetTexture(TextureType.FaceFront);
        } else if (Input.IsKeyDown(Keys.D)) {
            this.Sprite.SetTexture(TextureType.FaceRight);
        }
    }

    public void UpdateVelocity() {
        Vector2 angle = Vector2.Zero;

        if (Input.IsKeyDown(Keys.W)) {
            angle.Y--;
        }
        if (Input.IsKeyDown(Keys.A)) {
            angle.X--;
        }
        if (Input.IsKeyDown(Keys.S)) {
            angle.Y++;
        }
        if (Input.IsKeyDown(Keys.D)) {
            angle.X++;
        }

        if ( angle.LengthSquared() > 0 ) {
            this.DirectedMove(Math.Atan2(angle.Y, angle.X), this.Stats.RunSpeed);
        } else {
            this.StopMoving();
        }
    }

}

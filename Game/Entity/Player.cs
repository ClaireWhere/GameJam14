using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;


internal class Player : EntityActor {
    private static Player instance;
    public static Player Instance {
        get {
            return instance ??= new Player();
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
}

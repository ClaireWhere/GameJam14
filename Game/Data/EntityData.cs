using GameJam14.Game.Entity;
using GameJam14.Game.Entity.EntitySystem;

using Microsoft.Xna.Framework;

namespace GameJam14.Game.Data;
internal static class EntityData {
    public static Player Player {
        get {
            return Player.Instance;
        }
    }

    public static Enemy Tree {
        get {
            return new Enemy(
                id: 1,
                name: "Tree",
                position: new Vector2(200, 200),
                hitbox: new HitBox(
                    shape: new Shape.Rectangle(
                        source: Vector2.Zero,
                        width: SpriteData.TreeSprite.Texture.Width,
                        height: SpriteData.TreeSprite.Texture.Height,
                        scale: SpriteData.TreeSprite.Scale
                    ),
                    offset: new Vector2(
                        x: SpriteData.TreeSprite.Texture.Width / 2,
                        y: SpriteData.TreeSprite.Texture.Height / 2
                    )
                ),
                sprite: SpriteData.TreeSprite,
                baseStats: StatData.GetEnemyStats(EnemyData.EnemyType.Tree),
                inventory: new Inventory(),
                attack: new Attack(
                    attackRange: 0,
                    attackDistance: 0,
                    attackSpeed: 0,
                    attackCooldown: 2.0,
                    attackDamage: 10
                ),
                target: new Target(
                    type: Target.TargetType.Player,
                    targetRange: 500,
                    targetDistance: 200,
                    targetAngle: 0
                )
            );
        }
    }
}

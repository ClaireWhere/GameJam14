using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameJam14.Game.Entity;
using GameJam14.Game.Entity.EntitySystem;
using Microsoft.Xna.Framework;

namespace GameJam14.Game.Data;
static class EntityData {
    public static Enemy Tree => new Enemy(
        id: 1,
        name: "Tree",
        position: new Vector2(20, 20),
        hitbox: new List<Shape.Shape> {
            new Shape.Rectangle(new Vector2(20, 20), SpriteData.TreeSprite.Texture.Width, SpriteData.TreeSprite.Texture.Height)
        },
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

    public static Player Player => Player.Instance;
}

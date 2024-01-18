using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;
internal class Enemy : EntityActor {
    public Target Target { get; set; }

    public Enemy(int id, string name, Vector2 position, List<Shape.Shape> hitbox, Sprite sprite, Stats baseStats, Inventory inventory, Attack attack, Target target)
        : base(
            id: id,
            name: name,
            position: position,
            collision:
                new CollisionSource(
                    type: new CollisionType(CollisionType.SolidType.Solid, CollisionType.LightType.None, CollisionType.EntityType.Enemy, true, false),
                    hitbox: hitbox
                ),
            sprite: sprite,
            baseStats: baseStats,
            inventory: inventory,
            attack: attack
        ) {
        this.Target = target;
    }

}

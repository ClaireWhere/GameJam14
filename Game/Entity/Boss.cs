// Ignore Spelling: hitbox

using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace GameJam14.Game.Entity;
internal class Boss : Enemy {
    public Boss(int id, string name, Vector2 position, List<HitBox> hitboxes, Sprite sprite, Stats stats, Target target)
        : base(
            id: id,
            name: name,
            position: position,
            hitboxes: hitboxes,
            sprite: sprite,
            baseStats: stats,
            inventory: new Inventory(),
            attack: new Attack(),
            target: target
        ) {
    }

    public Boss(int id, string name, Vector2 position, HitBox hitbox, Sprite sprite, Stats stats, Target target)
        : base(
            id: id,
            name: name,
            position: position,
            hitbox: hitbox,
            sprite: sprite,
            baseStats: stats,
            inventory: new Inventory(),
            attack: new Attack(),
            target: target
        ) {
    }
}

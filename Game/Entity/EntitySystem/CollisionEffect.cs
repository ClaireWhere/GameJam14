using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity.EntitySystem;
internal class CollisionEffect
{
    public enum EntityInteraction
    {
        EnemyPlayer,
        PlayerEnemy,
        ProjectileEnemy,
        ProjectilePlayer,
        ProjectileProjectile,
        EnemyLight,
        PlayerLight,
        CloudPlayer,
        CloudEnemy
    }
    public CollisionEffect(Action<Entity, Entity> effect)
    {
        this.Effect = effect;
    }

    public Action<Entity, Entity> Effect { get; protected set; }
}

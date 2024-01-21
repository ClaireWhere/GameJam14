using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameJam14.Game.Entity;
using GameJam14.Game.Entity.EntitySystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam14.Game.GameSystem;
internal class EntityManager
{
    private readonly List<Entity.Entity> _entities = new List<Entity.Entity>();

    public void AddEntity(Entity.Entity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(Entity.Entity entity)
    {
        _entities.Remove(entity);
    }

    public Entity.Entity GetEntity(int id)
    {
        return _entities.Find(entity => entity.Id == id);
    }

    public void Update(GameTime gameTime)
    {
        foreach (Entity.Entity entity in _entities)
        {
            entity.Update(gameTime);
        }
        UpdateTargets();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entity.Entity entity in _entities)
        {
            entity.Draw(spriteBatch);
        }
    }

    public void UpdateTargets()
    {
        foreach (Enemy enemy in Enemies())
        {
            if (enemy.IsTargetInRange())
            {
                continue;
            }
            enemy.CurrentTarget = null;
            Debug.WriteLine("Lost target");
            if (enemy.Target.Type == Target.TargetType.Player)
            {
                float distance = Vector2.Distance(enemy.Position, Player().Position);
                if (enemy.Target.TargetRange >= distance)
                {
                    enemy.CurrentTarget = Player();
                    Debug.WriteLine("Targeting player");
                }
            }
            else if (enemy.Target.Type == Target.TargetType.Light)
            {
                float closestDistance = float.MaxValue;
                foreach (Light light in Lights())
                {
                    float distance = Vector2.Distance(enemy.Position, light.Position);
                    if (enemy.Target.TargetRange >= distance && distance < closestDistance)
                    {
                        closestDistance = distance;
                        enemy.CurrentTarget = light;
                    }
                }
            }
        }
    }

    public void Reset()
    {
        _entities.Clear();
    }

    public IEnumerable<Enemy> Enemies()
    {
        return _entities.OfType<Enemy>();
    }

    public IEnumerable<Light> Lights()
    {
        return _entities.OfType<Light>();
    }

    public Player Player()
    {
        return (Player)_entities.Find(entity => entity is Player);
    }
}

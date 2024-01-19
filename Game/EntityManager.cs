using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam14.Game;
internal class EntityManager {
    private readonly List<Entity.Entity> _entities = new List<Entity.Entity>();

    public void AddEntity(Entity.Entity entity) {
        _entities.Add(entity);
    }

    public void RemoveEntity(Entity.Entity entity) {
        _entities.Remove(entity);
    }

    public Entity.Entity GetEntity(int id) {
        return _entities.Find(entity => entity.Id == id);
    }

    public void Update(GameTime gameTime) {
        foreach ( Entity.Entity entity in _entities) {
            entity.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach ( Entity.Entity entity in _entities) {
            entity.Draw(spriteBatch);
        }
    }

    public void Reset() {
        _entities.Clear();
    }
}

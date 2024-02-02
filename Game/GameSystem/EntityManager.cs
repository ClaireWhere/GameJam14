﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using GameJam14.Game.Entity;
using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

namespace GameJam14.Game.GameSystem;
internal class EntityManager : IDisposable {
    /// <summary>
    ///   Initializes a new instance of the <see cref="EntityManager" /> class.
    /// </summary>
    public EntityManager() {
        this._entities = new List<Entity.Entity>();
        this._entityQueue = new List<Entity.Entity>();
        this._removeQueue = new List<Entity.Entity>();
        _spriteManager = new SpriteManager();
        this._disposed = false;
    }
    private bool _disposed;
    public void AddEntity(Entity.Entity entity) {
        _entityQueue.Add(entity);
    }

    public void RemoveEntity(Entity.Entity entity) {
        _removeQueue.Add(entity);
    }

    private void HandleCollisions() {
        bool[] handled = new bool[this._entities.Count];
        for ( int i = 0; i < this._entities.Count; i++ ) {
            if ( handled[i] ) {
                continue;
            }
            handled[i] = true;  // important to set to true here so we don't check the same entity with itself
            Entity.Entity entity = this._entities[i];
            for ( int j = i + 1; j < this._entities.Count; j++ ) {
                if ( handled[j] ) {
                    continue;
                }
                handled[j] = true;
                Entity.Entity otherEntity = this._entities[j];
                if ( entity.CheckCollision(otherEntity) ) {
                    Debug.WriteLine("Collision detected between " + entity.Id + " and " + otherEntity.Id);
                    entity.HandleCollision(otherEntity);
                    otherEntity.HandleCollision(entity);
                }
            }
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Draw(Camera camera) {
        this._spriteManager.Begin(camera);
        foreach ( Entity.Entity entity in _entities ) {
            this._spriteManager.Draw(entity);
        }
        this._spriteManager.End();
    }

    public IEnumerable<Enemy> Enemies() {
        return _entities.OfType<Enemy>();
    }

    // TODO: Remove this method - id is unsafe
    public Entity.Entity GetEntity(int id) {
        return _entities.Find(entity => entity.Id == id);
    }

    public IEnumerable<Light> Lights() {
        return _entities.OfType<Light>();
    }

    public Player Player() {
        return (Player) _entities.Find(entity => entity is Player);
    }

    public void Reset() {
        _entities.Clear();
        _entityQueue.Clear();
    }

    public void Update(GameTime gameTime) {
        this.ProcessEntityQueue();
        foreach ( Entity.Entity entity in _entities ) {
            entity.Update(gameTime);
        }
        this.UpdateTargets();
        this.HandleCollisions();
    }

    private void UpdateTargets() {
        foreach ( Enemy enemy in Enemies() ) {
            if ( enemy.IsTargetInRange() ) {
                continue;
            }
            enemy.CurrentTarget = null;
            if ( enemy.Target.Type == Target.TargetType.Player ) {
                float distance = Vector2.Distance(enemy.Position, Player().Position);
                if ( enemy.Target.TargetRange >= distance ) {
                    enemy.CurrentTarget = Player();
                    Debug.WriteLine("Targeting player");
                }
            } else if ( enemy.Target.Type == Target.TargetType.Light ) {
                float closestDistance = float.MaxValue;
                foreach ( Light light in Lights() ) {
                    float distance = Vector2.Distance(enemy.Position, light.Position);
                    if ( enemy.Target.TargetRange >= distance && distance < closestDistance ) {
                        closestDistance = distance;
                        enemy.CurrentTarget = light;
                    }
                }
            }
        }
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this._spriteManager.Dispose();
        this._disposed = true;
    }

    private readonly List<Entity.Entity> _entities;
    private readonly List<Entity.Entity> _entityQueue;
    private readonly List<Entity.Entity> _removeQueue;
    private readonly SpriteManager _spriteManager;
    private void ProcessEntityQueue() {
        this._entities.AddRange(this._entityQueue);
        foreach ( Entity.Entity entity in this._removeQueue ) {
            this._entities.Remove(entity);
        }
        this._entityQueue.Clear();
    }
}

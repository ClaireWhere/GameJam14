// Ignore Spelling: hitbox hitboxes

using System;
using System.Collections.Generic;

namespace GameJam14.Game.Entity.EntitySystem;
/// <summary>
///   Handles collisions between different types of objects and shapes.
/// </summary>
public class CollisionSource : IDisposable {
    public enum CollisionEffect {
        Damage,
        PreventMovement,
        DestroyLight,
        Heal,
        Kill,
        Stun,
        Slow,
        None
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="CollisionSource" /> class.
    /// </summary>
    /// <param name="type">
    ///   The type of the collision.
    /// </param>
    /// <param name="hitboxes">
    ///   The area of collision of the object.
    /// </param>
    public CollisionSource(CollisionType type, CollisionEffect collisionEffect, List<HitBox> hitboxes) {
        this.Type = type;
        this.Effects = new List<CollisionEffect>() { collisionEffect };
        this.Hitboxes = hitboxes;
        this._disposed = false;
    }

    public CollisionSource(CollisionType type, CollisionEffect collisionEffect, HitBox hitbox) {
        this.Type = type;
        this.Effects = new List<CollisionEffect>() { collisionEffect };
        this.Hitboxes = new List<HitBox>() { hitbox };
        this._disposed = false;
    }

    public CollisionSource(CollisionType type, List<CollisionEffect> collisionEffects, List<HitBox> hitboxes) {
        this.Type = type;
        this.Effects = collisionEffects;
        this.Hitboxes = hitboxes;
        this._disposed = false;
    }
    public CollisionSource(CollisionType type, List<CollisionEffect> collisionEffects, HitBox hitbox) {
        this.Type = type;
        this.Effects = collisionEffects;
        this.Hitboxes = new List<HitBox>() { hitbox };
        this._disposed = false;
    }

    private bool _disposed;

    public List<HitBox> Hitboxes { get; set; }

    /// <summary>
    ///   Gets or sets the type of the collision.
    /// </summary>
    public CollisionType Type { get; protected set; }
    public List<CollisionEffect> Effects { get; protected set; }

    /// <summary>
    ///   Checks whether a collision can occur between this source and another.
    /// </summary>
    /// <param name="other">
    ///   The other collision source.
    /// </param>
    /// <returns>
    ///   True if the two sources are able to collide, false otherwise.
    /// </returns>
    public bool CanCollideWith(CollisionSource other) {
        return this.Type.Collides(other.Type);
    }

    /// <summary>
    ///   Checks collision between this source and another.
    /// </summary>
    /// <param name="other">
    ///   The other collision source.
    /// </param>
    /// <returns>
    ///   True if the collision sources are able to collide and at least one collision occurs
    ///   between the hitboxes of the collision sources, false otherwise.
    /// </returns>
    public bool CollidesWith(CollisionSource other) {
        if ( !this.CanCollideWith(other) ) {
            return false;
        }

        for ( int i = 0; i < this.Hitboxes.Count; i++ ) {
            HitBox hitbox = this.Hitboxes[i];
            if ( other.CollidesWith(hitbox) ) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///   Checks collision between the hitboxes of this source and a shape.
    /// </summary>
    /// <param name="otherHitbox">
    ///   The shape of the hitbox to check collision with.
    /// </param>
    /// <returns>
    ///   True if at least one collision occurs between the shape and this collision source's
    ///   hitbox, false otherwise.
    /// </returns>
    public bool CollidesWith(HitBox otherHitbox) {
        for ( int i = 0; i < this.Hitboxes.Count; i++ ) {
            HitBox hitbox = this.Hitboxes[i];
            if ( hitbox.Shape.Intersects(otherHitbox.Shape) ) {
                return true;
            }
        }

        return false;
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }

        this._disposed = true;
    }

    public bool HasEffect(CollisionEffect effect) {
        return this.Effects.Contains(effect);
    }

    public void UpdateScale(float scale) {
        foreach ( HitBox hitbox in this.Hitboxes ) {
            hitbox.UpdateScale(scale);
        }
    }
}

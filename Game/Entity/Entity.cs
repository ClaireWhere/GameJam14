// Ignore Spelling: Teleport

using System;
using System.Diagnostics;

using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

namespace GameJam14.Game.Entity;
internal class Entity : IDisposable {
    public Entity(int id, Vector2 position, CollisionSource collision, Sprite sprite) {
        this.Id = id;
        this.Position = position;
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
        this.Collision = collision;
        this.Sprite = sprite;

        this.Destination = Vector2.Zero;
        this.IsTraveling = false;
        this._disposed = false;
    }
    public bool _disposed;
    public Vector2 Acceleration { get; set; }
    public CollisionSource Collision { get; set; }
    public Vector2 Destination { get; set; }
    public int Id { get; set; }
    public bool IsMoving { get { return this.Velocity != Vector2.Zero || this.Acceleration != Vector2.Zero; } }
    public bool IsTraveling { get; set; }
    public Vector2 Position {
		get {
			return this._position;
		}
		set {
			this._position = value;
			if ( this.Collision == null ) {
				return;
			}
			foreach ( Shape.Shape shape in this.Collision.Hitbox ) {
				shape.Position = value;
			}
		}
	}
	private Vector2 _position;
    public Sprite Sprite { get; set; }
    public Vector2 Velocity { get; set; }
    public bool CheckCollision(Entity entity) {
        return this.Collision.CollidesWith(entity.Collision);
    }

    /// <summary>
    /// Handles the collision between this and another entity considering this entity as the one being collided with (e.g., if this is a player and the other entity is a projectile, this method will handle the collision as if the player hit the projectile - rather than the projectile hitting the player).
    /// </summary>
    /// <param name="entity">The entity.</param>
    public virtual void HandleCollision(Entity entity) {
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Kill)) {
            Debug.WriteLine("Entity " + this.Id + " was killed by entity " + entity.Id);
            entity.Kill();
        }

        // TODO: Handle directional "stop moving" effect - currently, this will make the entity stuck
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.PreventMovement)) {
            entity.StopMoving();
        }
    }

    public float SlowMultiplier { get; set; }
    public float SlowDuration { get; set; }
    private float _slowTimer { get; set; }
    private bool IsSlowed { get { return _slowTimer > 0f; } }

    public float StunDuration { get; set; }
    private float _stunTimer { get; set; }
    public bool IsStunned { get { return _stunTimer > 0f; } }


    public void Slow(float duration, float amount) {
        if ( amount < 0f || amount > 1f) {
            throw new ArgumentOutOfRangeException(nameof(amount), "amount must be between 0 and 1");
        }
        if (duration < 0f) {
            throw new ArgumentOutOfRangeException(nameof(duration), "duration must be greater than 0");
        }

        this.SlowMultiplier = amount;
        this.SlowDuration = duration;
        this._slowTimer = 0f;
    }

    public void Stun(float duration) {
        if ( duration < 0f ) {
            throw new ArgumentOutOfRangeException(nameof(duration), "duration must be greater than 0");
        }
        this.StunDuration = duration;
        this._stunTimer = 0f;
    }

    /// <summary>
    ///   Sets the trajectory of the entity to the given destination with the given speed and
    ///   (optionally) acceleration.
    /// </summary>
    /// <param name="destination">
    ///   The destination.
    /// </param>
    /// <param name="speed">
    ///   The speed.
    /// </param>
    /// <param name="acceleration">
    ///   The acceleration.
    /// </param>
    public void DestinationMove(Vector2 destination, float speed, float acceleration = 0f) {
        this.Destination = destination;
        this.IsTraveling = true;

        // Calculate the direction of the destination
        Vector2 destinationDirection = new Vector2(destination.X - this.Position.X, destination.Y - this.Position.Y);
        destinationDirection.Normalize();

        // Set the magnitude of the velocity to the speed
        this.Velocity = destinationDirection * speed;
        this.Acceleration = destinationDirection * acceleration;
    }

    /// <summary>
    ///   Directs the entity to move in the given direction with the given speed and (optionally) acceleration.
    /// </summary>
    /// <param name="angle">
    ///   The angle in radians.
    /// </param>
    /// <param name="speed">
    ///   The speed.
    /// </param>
    /// <param name="acceleration">
    ///   The acceleration.
    /// </param>
    public void DirectedMove(double angle, float speed, float acceleration = 0f) {
        this.Velocity = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * speed;
        this.Acceleration = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * acceleration;
        this.IsTraveling = false;
    }

    public void DirectedMove(Vector2 angle, float speed, float acceleration = 0f) {
        this.Velocity = angle * speed;
        this.Acceleration = angle * acceleration;
        this.IsTraveling = false;
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Kill() {
        this.Dispose();
    }

    public void Move(GameTime gameTime) {
        if ( this.IsStunned ) {
            return;
        }

        if ( !this.IsMoving ) {
            return;
        }
        Vector2 projectedPosition = this.ProjectPosition(gameTime);
        // Debug.WriteLine("Projected position: " + projectedPosition);
        if ( this.IsTraveling ) {
            // Check if the entity is going to reach or pass its destination
            Shape.LineSegment movementPath = new Shape.LineSegment(this.Position, projectedPosition);
            if ( movementPath.Contains(this.Destination) ) {
                this.Position = this.Destination;
                this.StopTraveling();
                return;
            }
        }
        this.Position = projectedPosition;
        this.Velocity += this.Acceleration * (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public Vector2 ProjectPosition(GameTime gameTime) {
        Vector2 vComponent = this.Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 aComponent = this.Acceleration * (float) Math.Pow(gameTime.ElapsedGameTime.TotalSeconds, 2) / 2;
        if (this.IsSlowed) {
            vComponent *= this.SlowMultiplier;
            aComponent *= this.SlowMultiplier;
        }

        return this.Position + vComponent + aComponent;
    }

    public void StopMoving() {
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
        this.IsTraveling = false;
        this.Destination = Vector2.Zero;
    }

    public void StopTraveling() {
        this.IsTraveling = false;
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
    }

    /// <summary>
    ///   Teleports the entity to a given position.
    /// </summary>
    /// <param name="position">
    ///   The position.
    /// </param>
    public void TeleportTo(Vector2 position) {
        this.Position = position;
    }

    /// <summary>
    ///   Updates the sprite's position based on its velocity and acceleration.
    /// </summary>
    /// <param name="gameTime">
    ///   The game time.
    /// </param>
    public virtual void Update(GameTime gameTime) {
        this.UpdateSlow(gameTime);
        this.UpdateStun(gameTime);
        this.Move(gameTime);
    }

    private void UpdateSlow(GameTime gameTime) {
        if ( !this.IsSlowed ) {
            return;
        }
        this._slowTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ( this._slowTimer >= this.SlowDuration ) {
            this._slowTimer = 0f;
            this.SlowMultiplier = 1f;
            this.SlowDuration = 0f;
        }
    }

    private void UpdateStun(GameTime gameTime) {
        if ( !this.IsStunned ) {
            return;
        }
        this._stunTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ( this._stunTimer >= this.StunDuration ) {
            this._stunTimer = 0f;
            this.StunDuration = 0f;
        }
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this.Sprite.Dispose();
        this.Collision.Dispose();
        Game2.Instance().RemoveEntity(this);
        this._disposed = true;
    }
}

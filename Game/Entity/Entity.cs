// Ignore Spelling: Teleport

using GameJam14.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;
internal class Entity {
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Destination { get; set; }
    public bool IsTraveling { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public CollisionSource Collision { get; set; }
    public Sprite Sprite { get; set; }

    public Entity(int id, Vector2 position, CollisionSource collision, Sprite sprite ) {
        this.Id = id;
        this.Position = position;
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
        this.Collision = collision;
        this.Sprite = sprite;

        this.Destination = Vector2.Zero;
        this.IsTraveling = false;
    }

    /// <summary>
    /// Updates the sprite's position based on its velocity and acceleration.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public virtual void Update(GameTime gameTime) {
        this.Move(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        this.Sprite.Position = this.Position;
        this.Sprite.Draw(spriteBatch);
    }

    public virtual void Kill() {

    }
    public bool CheckCollision(Entity entity) {
        return this.Collision.CollidesWith(entity.Collision);
    }

    /// <summary>
    /// Teleports the entity to a given position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void TeleportTo(Vector2 position) {
        this.Position = position;
    }

    /// <summary>
    /// Directs the entity to move in the given direction with the given speed and (optionally) acceleration.
    /// </summary>
    /// <param name="angle">The angle in radians.</param>
    /// <param name="speed">The speed.</param>
    /// <param name="acceleration">The acceleration.</param>
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

    /// <summary>
    /// Sets the trajectory of the entity to the given destination with the given speed and (optionally) acceleration.
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="speed">The speed.</param>
    /// <param name="acceleration">The acceleration.</param>
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

    public void Move(GameTime gameTime) {
        if (!this.IsMoving) {
            return;
        }
        Vector2 projectedPosition = this.Position + ( this.Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds ) + ( this.Acceleration * (float) Math.Pow(gameTime.ElapsedGameTime.TotalSeconds, 2) / 2 );
        Debug.WriteLine("Projected position: " + projectedPosition);
        if (this.IsTraveling) {
            // Check if the entity is going to reach or pass its destination
            Shape.LineSegment movementPath = new Shape.LineSegment(this.Position, projectedPosition);
            if (movementPath.Contains(this.Destination)) {
                this.Position = this.Destination;
                this.StopTraveling();
                return;
            }
        }
        this.Position = projectedPosition;
        this.Velocity += this.Acceleration * (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void StopTraveling() {
        this.IsTraveling = false;
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
    }

    public void StopMoving() {
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
    }

    public bool IsMoving { get { return this.Velocity != Vector2.Zero || this.Acceleration != Vector2.Zero; } }
}

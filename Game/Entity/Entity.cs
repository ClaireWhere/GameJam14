using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;
internal class Entity {
    public int Id { get; set; }
    public Vector2 Position { get; set; }
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
    }

    /// <summary>
    /// Updates the sprite's position based on its velocity and acceleration.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public virtual void Update(GameTime gameTime) {
        this.Position += ( this.Velocity * (float) gameTime.ElapsedGameTime.TotalSeconds ) + ( this.Acceleration * (float) Math.Pow(gameTime.ElapsedGameTime.TotalSeconds, 2) / 2 );
        this.Velocity += this.Acceleration * (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Kill() {

    }
}

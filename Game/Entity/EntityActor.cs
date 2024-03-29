using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Diagnostics;

namespace GameJam14.Game.Entity;
internal class EntityActor : Entity {
    public EntityActor(int id, string name, Vector2 position, CollisionSource collision, Sprite sprite, Stats baseStats, Inventory inventory, Attack attack)
        : base(
                id: id,
                position: position,
                collision: collision,
                sprite: sprite
            ) {
        this.Name = name;
        this.BaseStats = baseStats;
        this.Inventory = inventory;
        this.Attack = attack;
        this.Modifiers = new List<Modifier>();
        this.Heal();
    }

    public override void HandleCollision(EntityActor actor) {
        Debug.WriteLine("\tEntityActor -> EntityActor Collision...");

        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Damage) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") was damaged by entity (" + actor.GetType().Name + ")");
            actor.TakeDamage(this.Attack.AttackDamage);
        }

        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Slow) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") was slowed by entity (" + actor.GetType().Name + ")");
            actor.Slow(this.Attack.SlowDuration, this.Attack.SlowMultiplier);
        }

        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Stun) ) {
            Debug.WriteLine("Entity (" + this.GetType().Name + ") was stunned by entity (" + actor.GetType().Name + ")");
            actor.Stun(this.Attack.StunDuration);
        }

        base.HandleCollision(actor);
    }

    private const float InvincibilityDuration = 0.5f;

    public Attack Attack { get; private set; }
    public Stats BaseStats { get; private set; }
    public int Health {
        get {
            return this._health;
        }
        private set {
            this._health = value < 0 ? 0 : value > this.Stats.Health ? this.Stats.Health : value;

            Debug.WriteLine("Entity (" + this.GetType().Name + ") health: " + this.Health + "/" + this.Stats.Health);
        }
    }
    private int _health;
    public float InvincibilityTimer { get; set; }
    public Inventory Inventory { get; private set; }
    public bool IsAlive { get { return this.Health > 0; } }
    public bool IsDamaged { get { return this.Health < this.Stats.Health; } }
    public bool IsDead { get { return this.Health <= 0; } }
    public bool IsFullHealth { get { return this.Health == this.Stats.Health; } }
    public List<Modifier> Modifiers { get; private set; }
    public Stats ModifierStats {
        get {
            Stats total = new Stats();
            foreach ( Modifier modifier in this.Modifiers ) {
                total.Add(modifier.Stats);
            }

            return total;
        }
    }

    public string Name { get; private set; }
    public Stats Stats {
        get {
            return this.BaseStats.Sum(this.Inventory.TotalStats).Sum(this.ModifierStats);
        }
    }

    public void AddModifier(Modifier modifier) {
        this.Modifiers.Add(modifier);
    }

    public void Heal(int healAmount) {
        this.Health += healAmount;
    }

    public void Heal() {
        this.Health = this.Stats.Health;
    }

    public void SetHealth(int health) {
        this.Health = health;
    }

    public void TakeDamage(int amount) {
        if ( this.InvincibilityTimer > 0f ) {
            return;
        }

        this.InvincibilityTimer = InvincibilityDuration;
        this.Health -= amount;
    }

    public override void Update(GameTime gameTime) {
        if ( this.IsDead ) {
            this.Kill();
            return;
        }

        this.UpdateInvincibility(gameTime);
        this.UpdateModifiers(gameTime);
        this.Attack.Update(gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    private void UpdateInvincibility(GameTime gameTime) {
        if ( this.InvincibilityTimer <= 0f ) {
            return;
        }

        this.InvincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if ( this.InvincibilityTimer < 0f ) {
            this.InvincibilityTimer = 0f;
        }
    }

    public void UpdateInstance(string name, Stats baseStats, Inventory inventory) {
        this.Name = name;
        this.BaseStats = baseStats;
        this.Inventory = inventory;
    }

    protected override void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }

        this.Stats.Dispose();
        this.Inventory.Dispose();
        this.Attack.Dispose();
        base.Dispose(disposing);
    }

    private void UpdateModifiers(GameTime gameTime) {
        foreach ( Modifier modifier in this.Modifiers ) {
            modifier.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            if ( modifier.IsExpired() ) {
                this.Modifiers.Remove(modifier);
            }
        }
    }
}

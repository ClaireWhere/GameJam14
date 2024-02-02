using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

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

    public void HandleCollision(EntityActor entity) {
        base.HandleCollision(entity);

        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Damage) ) {
            Debug.WriteLine("Entity " + this.Id + "(" + this.GetType().Name + ") was damaged by entity " + entity.Id + "(" + entity.GetType().Name + ")");
            entity.TakeDamage(this.Attack.AttackDamage);
        }
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Slow) ) {
            Debug.WriteLine("Entity " + this.Id + "(" + this.GetType().Name + ") was slowed by entity " + entity.Id + "(" + entity.GetType().Name + ")");
            entity.Slow(this.Attack.SlowDuration, this.Attack.SlowMultiplier);
        }
        if ( this.Collision.HasEffect(CollisionSource.CollisionEffect.Stun) ) {
            Debug.WriteLine("Entity " + this.Id + "(" + this.GetType().Name + ") was stunned by entity " + entity.Id + "(" + entity.GetType().Name + ")");
            entity.Stun(this.Attack.StunDuration);
        }

        base.HandleCollision(entity);
    }

    static float InvincibilityDuration = 0.5f;

    public Attack Attack { get; private set; }
    public Stats BaseStats { get; private set; }
    public int Health { get; private set; }
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
        if ( this.Health + healAmount > this.Stats.Health ) {
            this.Health = this.Stats.Health;
        } else {
            this.Health += healAmount;
        }
    }

    public void Heal() {
        this.SetHealth(this.Stats.Health);
    }

    public void SetHealth(int health) {
        this.Health = health > this.Stats.Health ? this.Stats.Health : health;
    }

    public void TakeDamage(int amount) {
        if ( this.InvincibilityTimer > 0f ) {
            return;
        }
        this.InvincibilityTimer = InvincibilityDuration;
        this.Health -= amount;
        if ( this.Health < 0 ) {
            this.Health = 0;
        }
    }

    public void TakeDamage(int amount) {
        if ( this.InvincibilityTimer > 0f ) {
            return;
        }
        this.InvincibilityTimer = InvincibilityDuration;
        this.Health -= amount;
        if ( this.Health < 0 ) {
            this.Health = 0;
        }
    }

    public override void Update(GameTime gameTime) {
        this.UpdateInvincibility(gameTime);
        this.UpdateModifiers(gameTime);
        this.Attack.Update(gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    private void UpdateInvincibility(GameTime gameTime) {
        if (this.InvincibilityTimer <= 0f) {
            return;
        }
        this.InvincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (this.InvincibilityTimer < 0f) {
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

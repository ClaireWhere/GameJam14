using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam14.Game.Entity;
internal class EntityActor : Entity {
    public string Name { get; private set; }
    public Stats BaseStats { get; private set; }
    public List<Modifier> Modifiers { get; private set; }
    public Inventory Inventory { get; private set; }
    public Attack Attack { get; private set; }
    public int Health { get; private set; }

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

    public void UpdateInstance(string name, Stats baseStats, Inventory inventory) {
        this.Name = name;
        this.BaseStats = baseStats;
        this.Inventory = inventory;
    }

    public override void Update(GameTime gameTime) {
        this.UpdateModifiers(gameTime);
        this.Attack.Update(gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    private void UpdateModifiers(GameTime gameTime) {
        foreach (Modifier modifier in this.Modifiers) {
            modifier.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (modifier.IsExpired()) {
                this.Modifiers.Remove(modifier);
            }
        }
    }

    public void Heal(int healAmount) {
        if (this.Health + healAmount > this.Stats.Health) {
            this.Health = this.Stats.Health;
        } else {
            this.Health += healAmount;
        }
    }

    public void Heal() {
        this.SetHealth(this.Stats.Health);
    }

    public void SetHealth(int health) {
        if ( health > this.Stats.Health ) {
            this.Health = this.Stats.Health;
        } else {
            this.Health = health;
        }
    }

    public bool IsDead { get { return this.Health <= 0; } }

    public bool IsAlive { get { return this.Health > 0; } }

    public bool IsFullHealth { get { return this.Health == this.Stats.Health; } }

    public bool IsDamaged { get { return this.Health < this.Stats.Health; } }

    public Stats Stats {
        get {
            return this.BaseStats.Sum(this.Inventory.TotalStats).Sum(this.ModifierStats);
        }
    }

    public Stats ModifierStats {
        get {
            Stats total = new Stats();
            foreach (Modifier modifier in this.Modifiers) {
                total.Add(modifier.Stats);
            }
            return total;
        }
    }

    public void AddModifier(Modifier modifier) {
        this.Modifiers.Add(modifier);
    }

}

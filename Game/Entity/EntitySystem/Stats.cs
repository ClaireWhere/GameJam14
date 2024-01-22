using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameJam14.Game;
internal class Stats : IDisposable {
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int IdleSpeed { get; set; }
    public int RunSpeed { get; set; }

    [JsonConstructorAttribute]
    public Stats(int health, int attack, int defense, int idleSpeed, int runSpeed) {
        Health = health;
        Attack = attack;
        Defense = defense;
        IdleSpeed = idleSpeed;
        RunSpeed = runSpeed;
    }

    public Stats() {
        Health = 0;
        Attack = 0;
        Defense = 0;
        IdleSpeed = 0;
        RunSpeed = 0;
    }

    public Stats Sum(Stats stats) {
        return new Stats(
            health: this.Health + stats.Health,
            attack: this.Attack + stats.Attack,
            defense: this.Defense + stats.Defense,
            idleSpeed: this.IdleSpeed + stats.IdleSpeed,
            runSpeed: this.RunSpeed + stats.RunSpeed
        );
    }

    public void Add(Stats stats) {
        this.Health += stats.Health;
        this.Attack += stats.Attack;
        this.Defense += stats.Defense;
        this.IdleSpeed += stats.IdleSpeed;
        this.RunSpeed += stats.RunSpeed;
    }

    public void TakeDamage(int damage) {
        if (this.Health - damage < 0) {
            this.Health = 0;
        } else {
            this.Health -= damage;
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        this.Dispose();
    }
}

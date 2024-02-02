using System;
using System.Text.Json.Serialization;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Stats : IDisposable {
	[JsonConstructor]
	public Stats(int health, int attack, int defense, int idleSpeed, int runSpeed) {
		this.Health = health;
		this.Attack = attack;
		this.Defense = defense;
		this.IdleSpeed = idleSpeed;
		this.RunSpeed = runSpeed;
		this._disposed = false;
	}

	public Stats() {
		this.Health = 0;
		this.Attack = 0;
		this.Defense = 0;
		this.IdleSpeed = 0;
		this.RunSpeed = 0;
		this._disposed = false;
	}

	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Health { get; set; }
	public int IdleSpeed { get; set; }
	public int RunSpeed { get; set; }
	public void Add(Stats stats) {
		this.Health += stats.Health;
		this.Attack += stats.Attack;
		this.Defense += stats.Defense;
		this.IdleSpeed += stats.IdleSpeed;
		this.RunSpeed += stats.RunSpeed;
	}

	public void Dispose() {
		this.Dispose(true);
		GC.SuppressFinalize(this);
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

	public void TakeDamage(int damage) {
		if ( this.Health - damage < 0 ) {
			this.Health = 0;
		} else {
			this.Health -= damage;
		}
	}

	protected virtual void Dispose(bool disposing) {
		if ( this._disposed ) {
			return;
		}
		this._disposed = true;
	}
	private bool _disposed;
}

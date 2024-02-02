using GameJam14.Game.Entity.EntitySystem;
using GameJam14.Game.Graphics;

using Microsoft.Xna.Framework;

using System;
using System.Text.Json.Serialization;

namespace GameJam14.Game.GameSystem;
internal class SaveData {
	[JsonConstructor]
	public SaveData(int currentArea, DateTime saveDate, PlayerData player) {
		this.CurrentArea = currentArea;
		this.SaveDate = saveDate;
		this.Player = player;
	}

	public SaveData(Entity.Player playerData, int currentArea) {
		this.Player = new PlayerData(playerData);
		this.CurrentArea = currentArea;
		this.SaveDate = DateTime.Now;
	}

	public int CurrentArea { get; private set; }
	public PlayerData Player { get; private set; }
	public DateTime SaveDate { get; private set; }
	public void Update(Entity.Player playerData, int currentArea) {
		this.Player = new PlayerData(playerData);
		this.CurrentArea = currentArea;
		this.SaveDate = DateTime.Now;
	}

	public void UpdatePlayer() {
		Entity.Player.UpdateInstance(
			name: this.Player.Name,
			position: new Vector2(this.Player.PositionX, this.Player.PositionY),
			currentTexture: this.Player.CurrentTexture,
			baseStats: this.Player.BaseStats,
			inventory: this.Player.Inventory,
			health: this.Player.Health
		);
	}

	public class PlayerData {
		[JsonConstructor]
		public PlayerData(string name, Stats baseStats, Inventory inventory, float positionX, float positionY, TextureType currentTexture, int health) {
			this.Name = name;
			this.BaseStats = baseStats;
			this.Inventory = inventory;
			this.PositionX = positionX;
			this.PositionY = positionY;
			this.CurrentTexture = currentTexture;
			this.Health = health;
		}

		public PlayerData(Entity.Player player) {
			this.Name = player.Name;
			this.BaseStats = player.BaseStats;
			this.Inventory = player.Inventory;
			this.PositionX = player.Position.X;
			this.PositionY = player.Position.Y;
			this.CurrentTexture = player.Sprite.TextureType;
			this.Health = player.Health;
		}

		public Stats BaseStats { get; set; }
		public TextureType CurrentTexture { get; set; }
		public int Health { get; set; }
		public Inventory Inventory { get; set; }
		public string Name { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public void Update(Entity.Player player) {
			this.Name = player.Name;
			this.BaseStats = player.BaseStats;
			this.Inventory = player.Inventory;
			this.PositionX = player.Position.X;
			this.PositionY = player.Position.Y;
			this.CurrentTexture = player.Sprite.TextureType;
			this.Health = player.Health;
		}
	}
}

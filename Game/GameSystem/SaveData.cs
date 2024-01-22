using System;
using System.Text.Json.Serialization;

using GameJam14.Game.Entity.EntitySystem;

using Microsoft.Xna.Framework;

namespace GameJam14.Game.GameSystem;
internal class SaveData {
    [JsonConstructor]
    public SaveData(int currentArea, DateTime saveDate, PlayerData player) {
        CurrentArea = currentArea;
        SaveDate = saveDate;
        Player = player;
    }

    public SaveData(Entity.Player playerData, int currentArea) {
        Player = new PlayerData(playerData);
        CurrentArea = currentArea;
        SaveDate = DateTime.Now;
    }

    public int CurrentArea { get; private set; }
    public PlayerData Player { get; private set; }
    public DateTime SaveDate { get; private set; }
    public void Update(Entity.Player playerData, int currentArea) {
        Player = new PlayerData(playerData);
        CurrentArea = currentArea;
        SaveDate = DateTime.Now;
    }

    public void UpdatePlayer() {
        Entity.Player.UpdateInstance(
            name: Player.Name,
            position: new Vector2(Player.PositionX, Player.PositionY),
            currentTexture: Player.CurrentTexture,
            baseStats: Player.BaseStats,
            inventory: Player.Inventory,
            health: Player.Health
        );
    }

    public class PlayerData {
        [JsonConstructor]
        public PlayerData(string name, Stats baseStats, Inventory inventory, float positionX, float positionY, TextureType currentTexture, int health) {
            Name = name;
            BaseStats = baseStats;
            Inventory = inventory;
            PositionX = positionX;
            PositionY = positionY;
            CurrentTexture = currentTexture;
            Health = health;
        }

        public PlayerData(Entity.Player player) {
            Name = player.Name;
            BaseStats = player.BaseStats;
            Inventory = player.Inventory;
            PositionX = player.Position.X;
            PositionY = player.Position.Y;
            CurrentTexture = player.Sprite.TextureType;
            Health = player.Health;
        }

        public Stats BaseStats { get; set; }
        public TextureType CurrentTexture { get; set; }
        public int Health { get; set; }
        public Inventory Inventory { get; set; }
        public string Name { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public void Update(Entity.Player player) {
            Name = player.Name;
            BaseStats = player.BaseStats;
            Inventory = player.Inventory;
            PositionX = player.Position.X;
            PositionY = player.Position.Y;
            CurrentTexture = player.Sprite.TextureType;
            Health = player.Health;
        }
    }
}

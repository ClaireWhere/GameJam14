using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace GameJam14.Game;
internal class SaveData {
    public class PlayerData {
        public string Name { get; set; }
        public Stats BaseStats { get; set; }
        public Inventory Inventory { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public TextureType CurrentTexture { get; set; }
        public int Health { get; set; }

        [JsonConstructorAttribute]
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
            Name = player.Name;
            BaseStats = player.BaseStats;
            Inventory = player.Inventory;
            PositionX = player.Position.X;
            PositionY = player.Position.Y;
            CurrentTexture = player.Sprite.TextureType;
            Health = player.Health;
        }

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

    public int CurrentArea { get; private set; }
    public DateTime SaveDate { get; private set; }
    public PlayerData Player { get; private set; }

    [JsonConstructorAttribute]
    public SaveData(int currentArea, DateTime saveDate, PlayerData player) {
        this.CurrentArea = currentArea;
        this.SaveDate = saveDate;
        this.Player = player;
    }

    public SaveData(Entity.Player playerData, int currentArea) {
        Player = new PlayerData(playerData);
        CurrentArea = currentArea;
        SaveDate = DateTime.Now;
    }

    public void Update(Entity.Player playerData, int currentArea) {
        Player = new PlayerData(playerData);
        CurrentArea = currentArea;
        SaveDate = DateTime.Now;
    }

    public Entity.Player GetPlayer() {
        return new Entity.Player(
            name: Player.Name,
            position: new Vector2(Player.PositionX, Player.PositionY),
            currentTexture: Player.CurrentTexture,
            baseStats: Player.BaseStats,
            inventory: Player.Inventory,
            health: Player.Health
        );
    }
}

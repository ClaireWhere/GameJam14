using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}

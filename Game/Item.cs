﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game;
internal class Item {
    public enum ItemType {
        Upgrade,
        Consumable,
        Weapon,
        Armor,
        Accessory
    }

    public ItemType Type { get; private set; }
    public Stats Stats { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Price { get; private set; }

    public Item(ItemType type, Stats stats, string name, string description, int price) {
        Type = type;
        this.Stats = stats;
        Name = name;
        Description = description;
        Price = price;
    }

    public Item(ItemType type, Stats stats, string name, string description) {
        Type = type;
        this.Stats = stats;
        Name = name;
        Description = description;
        Price = 0;
    }

}
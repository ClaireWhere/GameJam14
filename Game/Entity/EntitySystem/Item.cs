using System;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Item : IDisposable {
    public Item(ItemType type, Stats stats, string name, string description, int price) {
        this.Type = type;
        this.Stats = stats;
        this.Name = name;
        this.Description = description;
        this.Price = price;
        this._disposed = false;
    }

    public Item(ItemType type, Stats stats, string name, string description) {
        this.Type = type;
        this.Stats = stats;
        this.Name = name;
        this.Description = description;
        this.Price = 0;
        this._disposed = false;
    }

    public enum ItemType {
        Upgrade,
        Consumable,
        Weapon,
        Armor,
        Accessory
    }

    public string Description { get; private set; }
    public string Name { get; private set; }
    public int Price { get; private set; }
    public Stats Stats { get; private set; }
    public ItemType Type { get; private set; }
    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this.Stats.Dispose();
        this._disposed = true;
    }
    public bool _disposed;
}

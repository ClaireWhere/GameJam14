// Ignore Spelling: Calc

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Inventory : IDisposable {
    public Inventory() {
        Money = 0;
        Items = new List<Item>();
        this._disposed = false;
    }

    [JsonConstructor]
    public Inventory(int money, List<Item> items) {
        Money = money;
        Items = items;
        this._disposed = false;
    }
    public bool _disposed;
    public List<Item> Items { get; private set; }
    public int Money { get; private set; }
    public Stats TotalStats {
        get {
            return CalcTotalStats();
        }
    }

    public void AddItem(Item item) {
        Items.Add(item);
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public bool HasFunds(int amount) {
        return Money >= amount;
    }

    public void ReceivePaycheck(int amount) {
        Money += amount;
    }

    public void RemoveItem(Item item) {
        Items.Remove(item);
    }

    public void SpendMoney(int amount) {
        if ( HasFunds(amount) ) {
            Money -= amount;
        }
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this.Dispose();
        this._disposed = true;
    }

    private Stats CalcTotalStats() {
        Stats total = new Stats();
        foreach ( Item item in Items ) {
            total.Add(item.Stats);
        }
        return total;
    }
}

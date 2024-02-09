// Ignore Spelling: Calc

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Inventory : IDisposable {
    public Inventory() {
        this.Money = 0;
        this.Items = new List<Item>();
        this._disposed = false;
    }

    [JsonConstructor]
    public Inventory(int money, List<Item> items) {
        this.Money = money;
        this.Items = items;
        this._disposed = false;
    }
    public bool _disposed;
    public List<Item> Items { get; private set; }
    public int Money { get; private set; }
    public Stats TotalStats {
        get {
            return this.CalcTotalStats();
        }
    }

    public void AddItem(Item item) {
        this.Items.Add(item);
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public bool HasFunds(int amount) {
        return this.Money >= amount;
    }

    public void ReceivePaycheck(int amount) {
        this.Money += amount;
    }

    public void RemoveItem(Item item) {
        this.Items.Remove(item);
    }

    public void SpendMoney(int amount) {
        if ( this.HasFunds(amount) ) {
            this.Money -= amount;
        }
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }

        this._disposed = true;
    }

    private Stats CalcTotalStats() {
        Stats total = new Stats();
        foreach ( Item item in this.Items ) {
            total.Add(item.Stats);
        }

        return total;
    }
}

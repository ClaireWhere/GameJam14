// Ignore Spelling: Calc

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Inventory : IDisposable {
    public Inventory() {
        Money = 0;
        Items = new List<Item>();
    }

    [JsonConstructor]
    public Inventory(int money, List<Item> items) {
        Money = money;
        Items = items;
    }

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
        foreach ( Item item in this.Items ) {
            item.Dispose();
        }
        this.Dispose();
    }

    private Stats CalcTotalStats() {
        Stats total = new Stats();
        foreach ( Item item in Items ) {
            total.Add(item.Stats);
        }
        return total;
    }
}

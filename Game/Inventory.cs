// Ignore Spelling: Calc

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game;
internal class Inventory {
    public int Money { get; private set; }

    public List<Item> Items { get; private set; }

    public Inventory() {
        Money = 0;
        Items = new List<Item>();
    }

    public Inventory(int money, List<Item> items) {
        Money = money;
        Items = items;
    }

    public void ReceivePaycheck(int amount) {
        Money += amount;
    }

    public void SpendMoney(int amount) {
        if (this.HasFunds(amount)) {
            Money -= amount;
        }
    }

    public bool HasFunds(int amount) {
        return Money >= amount;
    }

    public void AddItem(Item item) {
        Items.Add(item);
    }

    public void RemoveItem(Item item) {
        Items.Remove(item);
    }

    private Stats CalcTotalStats() {
        Stats total = new Stats();
        foreach (Item item in Items) {
            total.Add(item.Stats);
        }
        return total;
    }
    public Stats TotalStats {
        get {
            return CalcTotalStats();
        }
    }
}

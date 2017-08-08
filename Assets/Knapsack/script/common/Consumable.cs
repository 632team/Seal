using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Consumable : Item
{
    public int BackHp { get; private set; }
    public int BackMp { get; private set; }

    public Consumable(int id, string name, string description, int buyPrice, int sellPrice, string icon, int backHp, int backMp)
        : base(id, name, description, buyPrice, sellPrice, icon)
    {
        this.BackHp = backHp;
        this.BackMp = backMp;
        base.ItemType = "Consumable";
    }
}


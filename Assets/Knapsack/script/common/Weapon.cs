using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class Weapon : Item
{
    public int Damage { get; private set; }
    public Weapon(int id, string name, string description, int buyPrice, int sellPrice, string icon, int damage)
        : base(id, name, description, buyPrice, sellPrice, icon)
    {
        this.Damage = damage;
        base.ItemType = "Weapon";
    }
}


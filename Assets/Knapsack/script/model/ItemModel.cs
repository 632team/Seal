using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{

    private static Dictionary<string, Item> GridItem = new Dictionary<string, Item>();
    public static void StoreItem(string name, Item item)
    {
        if (GridItem.ContainsKey(name))
        {
            DeleteItem(name);
        }
        GridItem.Add(name, item);
    }

    public static Item GetItem(string name)
    {
        if (GridItem.ContainsKey(name))
        {
            return GridItem[name];
        }
        else
        {
            return null;
        }
    }

    public static void DeleteItem(string name)
    {
        if (GridItem.ContainsKey(name))
        {
            GridItem.Remove(name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items = new List<Item>();

    void Start()
    {
        Items.Add(new Item("Sword"));
        Items.Add(new Item("Shield"));
        Items.Add(new Item("Potion"));

        Item found = FindItem("Potion");

        if (found != null)
            Debug.Log("찾은 아이템 : " + found.itemName);
        else
            Debug.Log("아이템을 찾을 수 없습니다.");
    }

    public Item FindItem(string itemName)
    {
        foreach(var  item in Items)
        {
            if(item.itemName == itemName) return item;
        }
        return null;
    }
}

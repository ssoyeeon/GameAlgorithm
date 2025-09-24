using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InventoryBigTest : MonoBehaviour
{
    List<Item> items = new List<Item>();

    private System.Random Random = new System.Random();
    void Start()
    {
        for(int i = 0; i< 100000; i++)
        {
            string name = $"Item_{i:05}";   //Item_0001 ����
            int qty = Random.Next(1, 100);
            items.Add(new Item(name, qty));
        }

        string target = "Item_45672";
        Stopwatch sw = Stopwatch.StartNew();
        Item foundLinear = FindItemLinear(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[���� Ž��] {target} ���� : {foundLinear?.quantity}, �ð� : {sw.ElapsedMilliseconds}ms");

        items.Sort((a,b) => a.itemName.CompareTo(b.itemName));

        sw.Restart();
        Item foundBinary = FindItembinary(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[���� Ž��] {target} ���� : {foundBinary?.quantity}, �ð� : {sw.ElapsedMilliseconds}ms");
    }
    public Item FindItemLinear(string targetName)
    {
        foreach(Item item in items)
        {
            if(item.itemName == targetName)
                return item;
        }    
        return null;
    }
    public Item FindItembinary(string targetName)
    {
        int left = 0;
        int right = items.Count - 1;

        while(left <= right)
        {
            int mid = (left + right) / 2;   
            int cmp = items[mid].itemName.CompareTo(targetName);

            if (cmp == 0) return items[mid];
            else if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

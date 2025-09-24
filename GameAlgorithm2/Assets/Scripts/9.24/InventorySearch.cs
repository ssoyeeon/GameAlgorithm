using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySearch : MonoBehaviour
{
    List<Item> items = new List<Item>();
    public TMP_Text itemText;
    public GameObject itemImage;

    private System.Random Random = new System.Random();

    void Start()
    {
        for (int i = 0; i <= 100; i++)
        {
            //�̹��� ���� �� �̹��� ���� �ؽ�Ʈ�� �̸� �ֱ�
            //�� �̸� ã�ƿͼ� ������ Ȯ��
            string name = $"Item_{i:03}";   //Item_0001 ����
            int qty = Random.Next(1, 100);
            items.Add(new Item(name, qty));
        }

    }

    public void FindLinear()
    {
        string target = itemText.text;
        Item foundLinear = FindItemLinear(target);
    }

    public void FindBinary()
    {
        string target = itemText.text;
        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));
        Item foundBinary = FindItembinary(target);
    }

    public Item FindItemLinear(string targetName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == targetName)
                return item;
        }
        return null;
    }

    public Item FindItembinary(string targetName)
    {
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int cmp = items[mid].itemName.CompareTo(targetName);

            if (cmp == 0) return items[mid];
            else if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return null;
    }
}

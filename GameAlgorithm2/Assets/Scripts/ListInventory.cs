using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(string itemName)
    {
        items.Add(new Item(itemName, 1));
        Debug.Log(itemName + " �߰��� (���� ����: " +  items.Count + ")");
    }

    public void RemoveItem(string itemName)
    {
        Item target = items.Find(x => x.itemName == itemName);
        if(target != null)
        {
            items.Remove(target);
            Debug.Log(itemName + "������ (���� ����: " + items.Count + ")");
        }
        else
        {
            Debug.Log(itemName + " �������� �����ϴ�. ");
        }
    }

    public void PrintInventory()
    {
        Debug.Log("=== ����Ʈ �κ��丮 ���� ===");
        if(items.Count == 0)
        {
            Debug.Log("�κ��丮�� ��� �ֽ��ϴ�.");
            return;
        }
        for(int i = 0; i < items.Count; i++)
        {
            Debug.Log(i + "�� ����: " + items[i].itemName + " x" + items[i].quantity);
        }
    }
    public void RemoveItemAll(string itemName)
    {
        int count = items.Count;
        items.RemoveAll(x => x.itemName == itemName);

        int removedCount = count - items.Count;

        if (removedCount > 0)
        {
            Debug.Log(itemName + " �������� ���� ������ (���� ��: " + removedCount + ", ���� ����: " + items.Count + ")");
        }
        else
        {
            Debug.Log(itemName + " �������� �����ϴ�.");
        }
    }
}

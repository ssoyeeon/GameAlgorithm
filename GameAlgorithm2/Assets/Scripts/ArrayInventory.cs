using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ArrayInventory : MonoBehaviour
{
    public int InventorySize = 10;

    public Item[] items;


    // Start is called before the first frame update
    void Start()
    {
        items = new Item[InventorySize];        
    }

    public void AddItem(string itemName)
    {
        for(int i = 0;  i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new Item(itemName, 1);
                Debug.Log(itemName + "추가 됨( 슬롯" + i + ")");
                return;
            }
            Debug.Log("아이템이 다 찼습니다.");
        }

    }

    public void RemoveItem(string itemName)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] != null && items[i].itemName == itemName)
            {
                Debug.Log(itemName + "삭제됨 (슬록" + i + ")");
                items[i] = null;
                return;
            }
        }
        Debug.Log(itemName + "아이템이 없습니다.");
    }

    public void PrintInventory()
    {
        Debug.Log("=== 배열 인벤토리 상태 ===");
        for(int i = 0; i < items.Length;i++)
        {
            if (items[i] != null)
                Debug.Log(i + "번 슬롯: " + items[i].itemName + "_x" + items[i].quantity);
            else
                Debug.Log(i + "번 슬롯: 비어있음");
                     
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

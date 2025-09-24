using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySearch : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField searchInput;     
    public Transform content;              
    public GameObject itemPrefab;          

    [Header("Generate Settings")]
    public int itemCount = 100;            
    public Vector2Int qtyRange = new Vector2Int(1, 100);

    private readonly List<Item> items = new List<Item>();
    private System.Random rng = new System.Random();

    void Start()
    {
        GenerateItems();
        DisplayAllItems();
    }

    void GenerateItems()
    {
        items.Clear();
        for (int i = 0; i < itemCount; i++)
        {
            string name = $"Item_{i:00}";
            int qty = rng.Next(qtyRange.x, qtyRange.y);
            items.Add(new Item(name, qty));
        }
    }

    void ClearContent()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);
    }

    void DisplayItems(List<Item> list)
    {
        ClearContent();

        for (int i = 0; i < list.Count; i++)
        {
            var go = Instantiate(itemPrefab, content, false);
            ItemView view = go.GetComponent<ItemView>();
            if (view != null) view.Set(list[i]);
        }
    }

    void DisplayAllItems()
    {
        DisplayItems(items);
    }

    Item FindItemLinear(string target)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (string.Equals(items[i].itemName, target, System.StringComparison.OrdinalIgnoreCase))
                return items[i];
        }
        return null;
    }

    Item FindItemBinary(string target)
    {
        List<Item> sorted = new List<Item>(items);
        sorted.Sort((a, b) => string.Compare(a.itemName, b.itemName, System.StringComparison.OrdinalIgnoreCase));

        int left = 0;
        int right = sorted.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int cmp = string.Compare(sorted[mid].itemName, target, System.StringComparison.OrdinalIgnoreCase);

            if (cmp == 0) return sorted[mid];
            if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return null;
    }

    public void OnClickSearchLinear()
    {
        string target = (searchInput != null ? searchInput.text : string.Empty).Trim();

        if (string.IsNullOrEmpty(target))
        {
            DisplayAllItems();
            return;
        }

        Item found = FindItemLinear(target);
        if (found != null)
        {
            List<Item> one = new List<Item>(1); one.Add(found);
            DisplayItems(one);
        }
        else ShowNotFound(target);
    }

    public void OnClickSearchBinary()
    {
        string target = (searchInput != null ? searchInput.text : string.Empty).Trim();

        if (string.IsNullOrEmpty(target))
        {
            DisplayAllItems();
            return;
        }

        Item found = FindItemBinary(target);
        if (found != null)
        {
            List<Item> one = new List<Item>(1); one.Add(found);
            DisplayItems(one);
        }
        else ShowNotFound(target);
    }

    void ShowNotFound(string target)
    {
        ClearContent();
        GameObject go = new GameObject("NotFound", typeof(RectTransform), typeof(TMP_Text));
        go.transform.SetParent(content, false);
        TMP_Text t = go.GetComponent<TMP_Text>();
        t.text = $"'{target}' NotFound";
        t.alignment = TextAlignmentOptions.Center;
        t.fontSize = 28f;
        t.color = Color.white;
    }
}

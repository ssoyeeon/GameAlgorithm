using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Search : MonoBehaviour
{
    [Header("UI Refrences (TMP)")]
    public TMP_InputField inputItemCount;
    public TMP_InputField inputSearchCount;
    public TMP_Text resultText;

    private List<Item> items = new List<Item>();

    private long sortSteps;         //���� �� Ƚ��
    private long linearSteps;       //���� Ž�� Ƚ��
    private long binarySteps;       //���� Ž�� Ƚ��

    public void OnFindButton()
    {
        if (!int.TryParse(inputItemCount.text, out int itemCount)) itemCount = 10000;
        if (!int.TryParse(inputSearchCount.text, out int searchCount)) searchCount = 100;

        items.Clear();
        for(int i = 0; i < itemCount; i++)
        {
            items.Add(new Item($"Item_{Random.Range(0, itemCount):D5}", 1));
        }

        List<string> targets = new List<string>();
        for(int i = 0; i < searchCount; i++)
        {
            targets.Add($"Item_{Random.Range(0, itemCount):D5}");
        }

        //1. ���� Ž��
        linearSteps = 0;
        foreach(var t in targets)
        {
            linearSteps += FindItemLinearSteps(t);
        }

        sortSteps = 0;
        QuickSort(items, 0, items.Count - 1);   //���� (�� Ƚ�� ��ϵ�)

        binarySteps = 0;
        foreach(var t in targets)
        {
            binarySteps += FindItemBinarySteps(t);
        }

        resultText.text =
            $"Item Count : {itemCount}\n" +
            $"Search Count : {searchCount}\n\n\n" +
            $"Linear Search Total Comparisons: {linearSteps}\n\n" +
            $"Quick Sort Comparisons : {sortSteps} \n" +
            $"Binary Search total Comparisons : {binarySteps}\n" +
            $"Total (Sort + Binary) : {sortSteps + binarySteps}";
    }

    private void QuickSort(List<Item> list, int left, int right)
    {
        if (left >= right) return;

        int pivotIndex = Partition(list, left, right);
        QuickSort(list, left, pivotIndex - 1);
        QuickSort(list, pivotIndex + 1, right);
    }

    private int Partition(List<Item> list, int left, int right)
    {
        Item pivot = list[right];
        int i = left - 1;

        for(int j = left; j <= right; j++)
        {
            sortSteps++;
            if (list[j].itemName.CompareTo(pivot.itemName) < 0)
            {
                i++;
                Swap(list, i, j);
            }
        }
        Swap(list, i + 1, right);
        return i + 1;
    }

    private void Swap(List<Item> list, int a, int b)
    {
        Item temp = list[a];
        list[a] = list[b];  
        list[b] = temp;
    }

    private int FindItemLinearSteps(string target)
    {
        int steps = 0;
        foreach(Item item in items)
        {
            steps++;
            if(item.itemName == target)
                return steps;
        }
        return steps;
    }

    private int FindItemBinarySteps(string target)
    {
        int steps = 0;
        int left = 0, right = items.Count - 1;

        while(left <= right)
        {
            steps++;
            int mid = (left + right) / 2;
            int cmp = items[mid].itemName.CompareTo(target);

            if (cmp == 0) return steps;
            else if(cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return steps;
    }
}


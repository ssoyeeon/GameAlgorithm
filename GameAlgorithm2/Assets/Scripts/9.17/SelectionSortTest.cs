using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSortTest : MonoBehaviour
{
    Stopwatch sw = new Stopwatch();
    public Text text;

    public void SelectionSort()
    {
        sw.Reset();
        sw.Start();
        int[] data = GenerateRandomArray(10000);
        StartSelectionSort(data);
        sw.Stop();
        long selectionTime = sw.ElapsedMilliseconds;
        text.text = ($"{selectionTime} ms\n");
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];

        System.Random rand = new System.Random();
        for(int i = 0;  i < size; i++)
        {
            arr[i] = rand.Next(0, 10000);
        }
        return arr;
    }

    public static void StartSelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for(int j = i + 1; j < n; j++)
            {
                if(arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }
            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;
        }
    }
}

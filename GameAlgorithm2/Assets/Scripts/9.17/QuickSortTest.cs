using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class QuickSortTest : MonoBehaviour
{
    Stopwatch sw = new Stopwatch();
    public Text text;
 
    public void QuickSort()
    {
        sw.Reset();
        sw.Start();
        int[] data = GenerateRandomArray(10000);
        StartQuickSort(data, 0, data.Length - 1);
        sw.Stop();
        long selectionTime = sw.ElapsedMilliseconds;
        text.text = ($"{selectionTime} ms");
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for(int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 100);
        }
        return arr; 
    }

    public static void StartQuickSort(int[] arr, int low, int high)
    {
        if(low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            StartQuickSort(arr, low, pivotIndex - 1);
            StartQuickSort(arr, pivotIndex + 1, high);
        }

    }

    public static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                //swap
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;
        return i + 1;
    }

}

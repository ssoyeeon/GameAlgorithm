using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSortTest : MonoBehaviour
{
    Stopwatch sw = new Stopwatch();
    public Text text;
    public void BubbleSort()
    {
        sw.Reset();
        sw.Start();
        int[] data = GenerateRandomArray(10000);
        StartBubbleSort(data);
        sw.Stop();
        long selectionTime = sw.ElapsedMilliseconds;
        text.text = ($"{selectionTime} ms\n");
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000);
        }
        return arr;
    }

    public static void StartBubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n; i++)
        {
            bool swapped = false;

            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[i] > arr[j + 1])
                {
                    //swap
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
                //이미 정렬된 경우 조기 종료 
                if (!swapped) break;
            }
        }
    }
}
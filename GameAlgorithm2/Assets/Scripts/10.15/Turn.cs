using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public static Turn Instance;
    public PQueueTest queueTest;
    public SimplePriorityQueue<string> queue = new SimplePriorityQueue<string>();
    public int i = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i++;
            if (queueTest.isTrun == true)
            {
                while (queue.Count > 0)
                {
                    Debug.Log($"{i}턴 / {queue.Dequeue()}의 턴입니다.");
                    queueTest.isTrun = false;
                }
            }
        }
    }
}

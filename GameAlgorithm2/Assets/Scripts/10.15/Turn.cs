using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public static Turn Instance;
    public SimplePriorityQueue<string> queue = new SimplePriorityQueue<string>();
    public int i = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i++;
            //Debug.Log($"{i}�� �Դϴ�.");
            while (queue.Count > 0)
            {
                Debug.Log($"{i}�� / {queue.Dequeue()}�� ���Դϴ�.");
            }
        }
    }
}

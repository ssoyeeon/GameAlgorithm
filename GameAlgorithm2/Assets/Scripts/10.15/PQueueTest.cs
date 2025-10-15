using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Search;
using UnityEngine;

public class PQueueTest : MonoBehaviour
{
    public static PQueueTest Instance;
    public Turn turn;

    public int speed;
    public string chacName;
    
    public int amount = 0;
    public bool isTrun;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            amount += speed;
            if(amount >= 100)
            {
                isTrun = true;
                amount -= 100;
                turn.queue.Enqueue(chacName, speed);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueMoving : MonoBehaviour
{
    public float speed = 5f;
    private Queue<Vector3> moveQueue;

    private bool isMoving = false;
    private Vector3 targetPos;

    private void Start()
    {
        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(!isMoving)
        {
            if(x != 0 || y != 0)
            {
                Vector3 move = new Vector3(x, y, 0).normalized * speed * Time.deltaTime;
                targetPos += move;
                moveQueue.Enqueue(targetPos);

            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(!isMoving && moveQueue.Count > 0)
                {
                    isMoving = true;
                }
            }
        }

        else
        {
            if(moveQueue.Count > 0)
            {
                transform.position = moveQueue.Dequeue();
                isMoving = false;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}

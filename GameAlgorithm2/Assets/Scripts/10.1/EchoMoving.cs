using System.Collections.Generic;
using UnityEngine;

public class EchoMoving : MonoBehaviour
{
    public float speed = 5f;

    private Queue<Vector3> recorded;   // 기록된 이동 델타들 (재생용)
    private Stack<Vector3> played;     // 방금 재생된 델타들 (역행용)

    private bool isRecording = false;
    private bool isPlaying = false;

    void Start()
    {
        recorded = new Queue<Vector3>();
        played = new Stack<Vector3>();
    }

    void Update()
    {
        if (isRecording && !isPlaying)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector3 delta = new Vector3(x, y, 0f) * speed * Time.deltaTime;

            if (delta.sqrMagnitude > 0f)
                recorded.Enqueue(delta);
        }
        else if (isPlaying && !isRecording)
        {
            bool rewinding = Input.GetKey(KeyCode.R);

            if (rewinding)
            {
                if (played.Count > 0)
                {
                    GetComponent<Renderer>().material.color = Color.blue;
                    Vector3 d = played.Pop();
                    transform.position -= d;
                }
            }
            else
            {
                if (recorded.Count > 0)
                {
                    Vector3 d = recorded.Dequeue();
                    transform.position += d;
                    played.Push(d);
                }
            }

            if (recorded.Count == 0 && played.Count == 0)
                isPlaying = false;
        }
    }

    public void OnClickRecord()
    {
        isPlaying = false;
        isRecording = true;
        recorded.Clear();
        played.Clear();
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void OnClickPlay()
    {
        isRecording = false;
        isPlaying = true;
        played.Clear();
    }
}

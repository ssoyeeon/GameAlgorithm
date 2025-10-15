using System.Collections.Generic;
using UnityEngine;

public class EcoMoving : MonoBehaviour
{
    public float speed = 5f;
    private Renderer rend;

    struct Cmd { public bool rewind; public Vector3 delta; public Cmd(bool r, Vector3 d) { rewind = r; delta = d; } }

    Queue<Cmd> script = new Queue<Cmd>();
    Stack<Vector3> played = new Stack<Vector3>();
    bool isRecording, isPlaying;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.white;
    }

    void Update()
    {
        rend.material.color = Color.white; // 기본은 항상 흰색

        if (isRecording)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            bool r = Input.GetKey(KeyCode.R);

            if (r) script.Enqueue(new Cmd(true, Vector3.zero));
            Vector3 d = new Vector3(x, y, 0f) * speed * Time.deltaTime;
            if (!r && d.sqrMagnitude > 0f) script.Enqueue(new Cmd(false, d));
        }
        else if (isPlaying)
        {
            if (Input.GetKey(KeyCode.R) && played.Count > 0)
            {
                transform.position -= played.Pop();
                rend.material.color = Color.blue;
                return;
            }

            if (script.Count > 0)
            {
                var c = script.Dequeue();
                if (c.rewind)
                {
                    if (played.Count > 0)
                    {
                        transform.position -= played.Pop();
                        rend.material.color = Color.blue;
                    }
                }
                else
                {
                    transform.position += c.delta;
                    played.Push(c.delta);
                }
            }
        }
    }

    public void OnClickRecord()
    {
        isPlaying = false;
        isRecording = true;
        script.Clear();
        played.Clear();
        rend.material.color = Color.white;
    }

    public void OnClickPlay()
    {
        isRecording = false;
        isPlaying = true;
        played.Clear();
        rend.material.color = Color.white;
    }
}

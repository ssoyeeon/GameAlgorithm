using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Das : MonoBehaviour
{
    public GameObject[] cube = new GameObject[10];
    public List<GameObject> gameObjects = new List<GameObject>();
    public Dictionary<int, string> keyValue = new Dictionary<int, string>();

    void Start()
    {
        keyValue.Add(1, "Wad");
    }

    // Update is called once per frame
    void Update()
    {

        //for (int i = 0; i < 10; i++)
        //{
        //    cube[i].transform.position += new Vector3(0,1,0);
        //}

        foreach(GameObject cuu in gameObjects)
        {
            cuu.transform.position += new Vector3(0, 1, 0);
        }

    }
}

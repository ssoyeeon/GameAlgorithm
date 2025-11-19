using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;

    public void StartMove(List<Vector2Int> path, float cellSize)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(path, cellSize));
    }

    IEnumerator MoveRoutine(List<Vector2Int> path, float cellSize)
    {
        foreach (var p in path)
        {
            Vector3 target = new Vector3(p.x * cellSize, 0.5f, p.y * cellSize);

            while ((transform.position - target).sqrMagnitude > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    Time.deltaTime * moveSpeed
                );
                yield return null;
            }
        }
    }
}


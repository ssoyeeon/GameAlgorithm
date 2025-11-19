using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSystem : MonoBehaviour
{
    public int width = 21;      // 홀수 추천
    public int height = 21;
    public float cellSize = 1f; // 큐브 크기

    int[,] map;
    Transform root;

    Vector2Int start = new Vector2Int(1, 1);
    Vector2Int goal;

    GameObject[,] tiles;
    public PlayerMove playerMove;

    Vector2Int[] dirs = {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    void Start()
    {
        GenerateAndVisualize();
        DrawShortestPath();
    }

    public void GenerateAndVisualize()
    {
        if (root != null) Destroy(root.gameObject);
        root = new GameObject("Maze").transform;

        while (true)
        {
            map = GenerateMaze(width, height);
            goal = new Vector2Int(width - 2, height - 2);

            if (CanEscapeDFS())
                break;
        }

        Visualize();
    }

    // DFS 백트래킹 미로 생성
    int[,] GenerateMaze(int w, int h)
    {
        int[,] maze = new int[w, h];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                maze[x, y] = 1;

        Stack<Vector2Int> st = new Stack<Vector2Int>();
        Vector2Int cur = start;
        maze[cur.x, cur.y] = 0;
        st.Push(cur);

        System.Random rand = new System.Random();

        while (st.Count > 0)
        {
            cur = st.Pop();
            List<Vector2Int> nextList = new List<Vector2Int>();

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x * 2;
                int ny = cur.y + d.y * 2;
                if (InBounds(nx, ny) && maze[nx, ny] == 1)
                    nextList.Add(new Vector2Int(nx, ny));
            }

            if (nextList.Count > 0)
            {
                st.Push(cur);

                Vector2Int next = nextList[rand.Next(nextList.Count)];
                int mx = (cur.x + next.x) / 2;
                int my = (cur.y + next.y) / 2;

                maze[mx, my] = 0;
                maze[next.x, next.y] = 0;

                st.Push(next);
            }
        }

        return maze;
    }

    bool InBounds(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < width && y < height);
    }

    // 탈출 가능 DFS 검사
    bool CanEscapeDFS()
    {
        bool[,] visited = new bool[width, height];
        return DFS(start.x, start.y, visited);
    }

    bool DFS(int x, int y, bool[,] visited)
    {
        if (x == goal.x && y == goal.y) return true;
        visited[x, y] = true;

        foreach (var d in dirs)
        {
            int nx = x + d.x;
            int ny = y + d.y;

            if (!InBounds(nx, ny)) continue;
            if (map[nx, ny] == 1) continue;
            if (visited[nx, ny]) continue;

            if (DFS(nx, ny, visited)) return true;
        }

        return false;
    }

    void Visualize()
    {
        tiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
                cube.transform.localScale = Vector3.one * cellSize;
                cube.transform.SetParent(root);

                var mat = cube.GetComponent<Renderer>().material;
                if (map[x, y] == 1)
                    mat.color = Color.red;   // 벽
                else
                    mat.color = Color.white; // 바닥

                tiles[x, y] = cube;
            }
        }

        playerMove.transform.position = new Vector3(start.x * cellSize, 0.5f, start.y * cellSize);
    }

    public void DrawShortestPath()
    {
        List<Vector2Int> path = BFSShortestPath();

        if (path == null)
        {
            Debug.Log("경로 없음");
            return;
        }

        foreach (var p in path)
        {
            tiles[p.x, p.y].GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    List<Vector2Int> BFSShortestPath()
    {
        bool[,] visited = new bool[width, height];
        Vector2Int?[,] parent = new Vector2Int?[width, height];

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(start);
        visited[start.x, start.y] = true;

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();

            if (cur == goal)
                return BuildPath(parent);

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(nx, ny)) continue;
                if (map[nx, ny] == 1) continue;
                if (visited[nx, ny]) continue;

                visited[nx, ny] = true;
                parent[nx, ny] = cur;
                q.Enqueue(new Vector2Int(nx, ny));
            }
        }
        return null;
    }

    List<Vector2Int> BuildPath(Vector2Int?[,] parent)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goal;

        while (cur.HasValue)
        {
            path.Add(cur.Value);
            cur = parent[cur.Value.x, cur.Value.y];
        }

        path.Reverse();
        return path;
    }
    public void AutoMove()
    {
        List<Vector2Int> path = BFSShortestPath();

        if (path == null)
        {
            return;
        }

        playerMove.StartMove(path, cellSize);
    }
}

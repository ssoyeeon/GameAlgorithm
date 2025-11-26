using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int width = 11;  // 홀수 추천
    public int height = 11; // 홀수 추천
    public Vector2Int startPos = new Vector2Int(1, 1);
    public Vector2Int goalPos = new Vector2Int(9, 9);

    [Header("Rendering")]
    public float tileSize = 1.0f;

    // 맵 데이터 (0:벽, 1:평지, 2:숲, 3:진흙)
    private int[,] map;

    // 생성된 오브젝트들을 담아둘 리스트 (재생성 시 삭제용)
    private List<GameObject> mapObjects = new List<GameObject>();
    private List<GameObject> pathObjects = new List<GameObject>();

    void Start()
    {
        // 시작하자마자 맵 생성 시도
        GenerateValidMap();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 40), "맵 재생성 (DFS 검증)"))
        {
            GenerateValidMap();
        }

        if (GUI.Button(new Rect(10, 60, 150, 40), "길 안내 (Dijkstra)"))
        {
            VisualizeShortestPath();
        }
    }

    // --- 1. 맵 생성 및 유효성 검사 로직 ---

    void GenerateValidMap()
    {
        int attempt = 0;
        bool possible = false;

        do
        {
            GenerateRandomData();
            // DFS로 탈출 가능한지 확인 (비용 무시, 벽만 체크)
            possible = CheckPathDFS(startPos, goalPos);
            attempt++;
        }
        while (!possible && attempt < 1000); // 무한루프 방지용 1000회 제한

        if (possible)
        {
            DrawMap();
        }
        else
        {
            Debug.LogError("탈출 가능한 맵 생성 실패 (맵 크기나 장애물 비율 조절 필요)");
        }
    }

    void GenerateRandomData()
    {
        map = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 1. 가장자리는 무조건 벽(0)
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x, y] = 0;
                }
                else
                {
                    int rand = Random.Range(0, 100);

                    if (rand < 25) map[x, y] = 0;      // 벽
                    else if (rand < 60) map[x, y] = 1; // 평지 (코스트 1)
                    else if (rand < 80) map[x, y] = 2; // 숲 (코스트 3)
                    else map[x, y] = 3;                // 진흙 (코스트 5)
                }
            }
        }

        // 시작점과 도착점은 무조건 평지로 뚫어줌
        map[startPos.x, startPos.y] = 1;
        map[goalPos.x, goalPos.y] = 1;
    }

    // --- 2. DFS (탈출 여부 파악) ---
    bool CheckPathDFS(Vector2Int current, Vector2Int target)
    {
        bool[,] visited = new bool[width, height];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        stack.Push(current);
        visited[current.x, current.y] = true;

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (stack.Count > 0)
        {
            Vector2Int cur = stack.Pop();

            if (cur == target) return true; // 도착

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                // 범위 체크
                if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;
                // 벽 체크 (0이면 못감)
                if (map[nx, ny] == 0) continue;
                // 방문 체크
                if (visited[nx, ny]) continue;

                visited[nx, ny] = true;
                stack.Push(new Vector2Int(nx, ny));
            }
        }

        return false; // 길 없음
    }

    // --- 3. Dijkstra (최단거리 탐색) ---
    void VisualizeShortestPath()
    {
        // 기존 경로 시각화 삭제
        foreach (var obj in pathObjects) Destroy(obj);
        pathObjects.Clear();

        List<Vector2Int> path = Dijkstra(startPos, goalPos);

        if (path == null)
        {
            Debug.Log("경로를 찾을 수 없습니다.");
            return;
        }

        // 경로 시각화 (작은 빨간 구체로 표시)
        foreach (var p in path)
        {
            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dot.transform.position = new Vector3(p.x * tileSize, 1.5f, p.y * tileSize); // 맵보다 약간 위에
            dot.transform.localScale = Vector3.one * 0.4f;
            dot.GetComponent<Renderer>().material.color = Color.red;
            pathObjects.Add(dot);
        }
        Debug.Log($"최단 경로 길이: {path.Count}");
    }

    List<Vector2Int> Dijkstra(Vector2Int start, Vector2Int goal)
    {
        int[,] dist = new int[width, height];
        Vector2Int?[,] parent = new Vector2Int?[width, height];
        bool[,] visited = new bool[width, height]; // 방문 체크 추가

        // 거리 초기화
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                dist[x, y] = int.MaxValue;

        dist[start.x, start.y] = 0;

        List<Vector2Int> openList = new List<Vector2Int>();
        openList.Add(start);

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (openList.Count > 0)
        {
            // 1. 최소 거리 노드 찾기 (우선순위 큐 역할)
            int bestIndex = 0;
            int minDistance = int.MaxValue;

            for (int i = 0; i < openList.Count; i++)
            {
                int d = dist[openList[i].x, openList[i].y];
                // [수정됨] 기존 코드의 if (d > bestDist)는 거리가 먼 것을 찾으므로 잘못됨.
                // 가장 작은 거리(d < minDistance)를 찾아야 함.
                if (d < minDistance)
                {
                    minDistance = d;
                    bestIndex = i;
                }
            }

            Vector2Int u = openList[bestIndex];
            openList.RemoveAt(bestIndex);

            if (u == goal) return ReconstructPath(parent, start, goal);
            if (visited[u.x, u.y]) continue; // 이미 처리된 노드 스킵
            visited[u.x, u.y] = true;

            // 2. 이웃 노드 탐색
            foreach (var d in dirs)
            {
                int nx = u.x + d.x;
                int ny = u.y + d.y;

                if (!InBounds(nx, ny)) continue;
                if (map[nx, ny] == 0) continue; // 벽이면 못감

                int cost = TileCost(map[nx, ny]);
                int newDist = dist[u.x, u.y] + cost;

                if (newDist < dist[nx, ny])
                {
                    dist[nx, ny] = newDist;
                    parent[nx, ny] = u;

                    // 아직 방문하지 않았고, 오픈 리스트에 없다면 추가
                    // (단순 리스트 구현이라 중복 추가를 막기 위해 Contains 체크하거나
                    //  visited 체크로 나중에 걸러냄. 여기선 Contains 체크로 최적화)
                    if (!openList.Contains(new Vector2Int(nx, ny)))
                    {
                        openList.Add(new Vector2Int(nx, ny));
                    }
                }
            }
        }
        return null;
    }

    // --- 4. 헬퍼 함수 및 시각화 ---

    void DrawMap()
    {
        // 기존 맵 오브젝트 삭제
        foreach (var obj in mapObjects) Destroy(obj);
        mapObjects.Clear();
        foreach (var obj in pathObjects) Destroy(obj);
        pathObjects.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);
                tile.transform.localScale = Vector3.one * tileSize * 0.9f; // 약간 틈 주기

                Renderer rd = tile.GetComponent<Renderer>();
                int type = map[x, y];

                switch (type)
                {
                    case 0: // 벽
                        rd.material.color = Color.black;
                        tile.transform.localScale += Vector3.up; // 벽은 좀 높게
                        break;
                    case 1: // 평지 (코스트 1)
                        rd.material.color = Color.white;
                        break;
                    case 2: // 숲 (코스트 3)
                        rd.material.color = new Color(0, 0.5f, 0); // 짙은 녹색
                        break;
                    case 3: // 진흙 (코스트 5)
                        rd.material.color = new Color(0.6f, 0.4f, 0.2f); // 갈색
                        break;
                }

                // 시작/도착 지점 표시
                if (x == startPos.x && y == startPos.y) rd.material.color = Color.green;
                if (x == goalPos.x && y == goalPos.y) rd.material.color = Color.blue;

                mapObjects.Add(tile);
            }
        }
    }

    int TileCost(int type)
    {
        switch (type)
        {
            case 1: return 1; // 평지
            case 2: return 3; // 숲
            case 3: return 5; // 진흙
            default: return 9999;
        }
    }

    bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    List<Vector2Int> ReconstructPath(Vector2Int?[,] parent, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? curr = goal;

        while (curr.HasValue)
        {
            path.Add(curr.Value);
            if (curr.Value == start) break;
            curr = parent[curr.Value.x, curr.Value.y];
        }
        path.Reverse();
        return path;
    }
}
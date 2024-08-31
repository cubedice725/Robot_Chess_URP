using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMove : AStar
{
    private IObjectPool<PlayerMovePlane> playerMovePlanePool;
    private GameObject playerMovePlanePrefab;
    private GameObject playerPlaneStandard;
    private List<PlayerMovePlane> playerMovePlaneList = new List<PlayerMovePlane>();
    public RaycastHit Hit { get; set; }

    // 이동거리
    private int radiusMove = 4;

    private int movePlaneSetCount = 1000;
    
    int playerMovePlaneListCount;
    

    protected override void Awake()
    {
        base.Awake();
        playerPlaneStandard = GameObject.Find("PlayerPlaneStandard");
        // 플래이어 판 생성
        GameObject playerMovePlanePrefab = Resources.Load("Prefab/PlayerMovePlane", typeof(GameObject)) as GameObject;

        playerMovePlanePool = new ObjectPool<PlayerMovePlane>
            (
            CreatePlayerMovePlane, 
            OnGetPlayerMovePlane, 
            OnReleasePlayerMovePlane, 
            OnDestroyPlayerMovePlane,
            maxSize: 500
            );

        for (int i = 0; i < movePlaneSetCount; i++)
        {
            playerMovePlaneList.Add(playerMovePlanePool.Get());
            playerMovePlaneList[i].SetActive(false);
        }
    }
    protected override void SetPathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, z 대입
        sizeX = topRight.x - bottomLeft.x;
        sizeZ = topRight.z - bottomLeft.z;
        NodeArray = new Node[sizeX, sizeZ];

        for (int i = 0; i < sizeX * sizeZ; i++)
        {
            int map2dObject = 0;
            int map2dX = (int)transform.position.x + ((i / (radiusMove * 2 + 1)) - radiusMove);
            int map2dZ = (int)transform.position.z + ((i % (radiusMove * 2 + 1)) - radiusMove);

            try
            {
                map2dObject = gameSupporter.Map2D[map2dX, map2dZ];
            } catch { }

            bool isWall = false;
            if ((int)GameSupporter.map2DObject.wall == map2dObject || (int)GameSupporter.map2DObject.moster == map2dObject)
            {
                isWall = true;
            }

            NodeArray[i / sizeZ, i % sizeZ] = new Node(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.z - bottomLeft.z];
    }

    public void SetPlayerPlane()
    {
        playerMovePlaneListCount = 0;
        Vector3Int adj = new Vector3Int((int)playerPlaneStandard.transform.position.x - radiusMove, 0, (int)playerPlaneStandard.transform.position.z - radiusMove);
        // 지름 계산
        int diameter = radiusMove * 2 + 1;
        for (int i = 0; i < diameter * diameter; i++)
        {
            int X = (i / diameter);
            int Z = (i % diameter);
            try
            {
                if (X != radiusMove || Z != radiusMove)
                {
                    if (gameSupporter.Map2D[adj.x + X, adj.z + Z] != (int)GameSupporter.map2DObject.wall && gameSupporter.Map2D[adj.x + X, adj.z + Z] != (int)GameSupporter.map2DObject.moster)
                    {
                        if (Mathf.FloorToInt(Pythagoras(X - radiusMove, Z - radiusMove)) <= radiusMove)
                        {
                            PathFinding(
                                new Vector3Int(radiusMove, 0, radiusMove),
                                new Vector3Int(X, 0, Z),
                                Vector3Int.zero,
                                new Vector3Int(radiusMove * 2 + 1, 0, radiusMove * 2 + 1)
                            );
                            if (FinalNodeList.Count > 1 && FinalNodeList.Count <= radiusMove + 1)
                            {
                                playerMovePlaneList[playerMovePlaneListCount].transform.localPosition = new Vector3(X - radiusMove, -0.49f, Z - radiusMove);
                                playerMovePlaneList[playerMovePlaneListCount].SetActive(true);

                                playerMovePlaneListCount++;
                            }
                        }
                    }
                }
            } catch { }
        }
    }
    public void Move()
    {
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2DObject.noting;

        transform.position = new Vector3(Hit.transform.position.x, transform.position.y, Hit.transform.position.z);
        playerPlaneStandard.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        RemovePlayerPlane();

        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2DObject.player;
    }
    protected override bool OpenListAddCondition(int checkX, int checkZ)
    {
        // 플레이어가 움직이는 상하좌우 범위를 벗어나지 않고
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z)
        {
            // 맵을 벗어나지 않고
            if ((int)transform.position.x + (checkX - radiusMove) < gameSupporter.MapSizeX && (int)transform.position.z + (checkZ - radiusMove) < gameSupporter.MapSizeZ)
            {
                if ((int)transform.position.x + (checkX - radiusMove) >= 0 && (int)transform.position.z + (checkZ - radiusMove) >= 0)
                {
                    // 벽이 아니면서, 닫힌리스트에 없다면
                    if (!NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    // 플래이어 판 회수
    public void RemovePlayerPlane()
    {
        if (playerMovePlaneListCount != 0)
        {
            for (int i = 0; i < playerMovePlaneListCount; i++)
            {
                playerMovePlaneList[i].gameObject.transform.position = new Vector3(0, -100, 0);
                playerMovePlaneList[i].SetActive(false);
            }
        }
    }

    private float Pythagoras(int pythA, int pythB)
    {
        return Mathf.Sqrt((pythA * pythA) + (pythB * pythB));
    }

    private PlayerMovePlane CreatePlayerMovePlane()
    {
        PlayerMovePlane plane = Instantiate(playerMovePlanePrefab).GetComponent<PlayerMovePlane>();
        plane.SetManagedPool(playerMovePlanePool);
        return plane;
    }
    private void OnGetPlayerMovePlane(PlayerMovePlane plane)
    {
        plane.gameObject.SetActive(false);
    }
    private void OnReleasePlayerMovePlane(PlayerMovePlane plane)
    {
        plane.gameObject.SetActive(false);
    }
    private void OnDestroyPlayerMovePlane(PlayerMovePlane plane)
    {
        Destroy(plane.gameObject);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : AStar
{
    private int movePlaneSetCount = 1000;
    private int radiusMove = 4;
    int movePlaneInstListCount;

    public RaycastHit Hit { get; set; }
    protected List<GameObject> movePlaneInstList = new List<GameObject>();
    protected override void Awake()
    {
        base.Awake();
        GameObject movePlaneprefab = Resources.Load("Prefab/Move Plane", typeof(GameObject)) as GameObject;
        for (int i = 0; i < movePlaneSetCount; i++)
        {
            movePlaneInstList.Add(Instantiate(movePlaneprefab, transform));
            movePlaneInstList[i].transform.position = new Vector3(0, -100, 0);
            movePlaneInstList[i].SetActive(false);
        }
    }
    protected override void SetPathFinding()
    {
        // NodeArray�� ũ�� �����ְ�, isWall, x, z ����
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
            }
            catch
            {

            }

            bool isWall = false;
            if ((int)GameSupporter.map2dObject.wall == map2dObject || (int)GameSupporter.map2dObject.moster == map2dObject)
            {
                isWall = true;
            }

            NodeArray[i / sizeZ, i % sizeZ] = new Node(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }
    
        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.z - bottomLeft.z];
    }

    public void setPlayerPlane()
    {
        movePlaneInstListCount = 0;
        Vector3Int adj = new Vector3Int((int)transform.position.x - radiusMove, 0, (int)transform.position.z - radiusMove);
        // ���� ���
        int diameter = radiusMove * 2 + 1;
        for (int i = 0; i < diameter * diameter; i++)
        {
            int X = (i / diameter);
            int Z = (i % diameter);
            try
            {
                if (X != radiusMove || Z != radiusMove)
                {
                    if (gameSupporter.Map2D[adj.x + X, adj.z + Z] != (int)GameSupporter.map2dObject.wall && gameSupporter.Map2D[adj.x + X, adj.z + Z] != (int)GameSupporter.map2dObject.moster)
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
                                movePlaneInstList[movePlaneInstListCount].transform.localPosition = new Vector3(X - radiusMove, -0.49f, Z - radiusMove);
                                movePlaneInstList[movePlaneInstListCount].SetActive(true);

                                movePlaneInstListCount++;

                            }
                        }
                    }
                }
            }
            catch { }

        }
    }
    public void Move()
    {
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.noting;

        transform.position = new Vector3(Hit.transform.position.x, transform.position.y, Hit.transform.position.z);

        for (int i = 0; i < movePlaneInstListCount; i++)
        {
            movePlaneInstList[i].transform.position = new Vector3(0, -100, 0);
            movePlaneInstList[i].SetActive(false);
        }
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.player;
    }
    protected override bool OpenListAddCondition(int checkX, int checkZ)
    {
        // �÷��̾ �����̴� �����¿� ������ ����� �ʰ�
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z)
        {
            // ���� ����� �ʰ�
            if ((int)transform.position.x + (checkX - radiusMove) < gameSupporter.MapSizeX && (int)transform.position.z + (checkZ - radiusMove) < gameSupporter.MapSizeZ)
            {
                if ((int)transform.position.x + (checkX - radiusMove) >= 0 && (int)transform.position.z + (checkZ - radiusMove) >= 0)
                {
                    // ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
                    if (!NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private float Pythagoras(int pythA, int pythB)
    {
        return Mathf.Sqrt((pythA * pythA) + (pythB * pythB));
    }
}

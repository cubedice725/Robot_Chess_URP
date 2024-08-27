using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node
{
    public Node ParentNode;

    public bool isWall;
    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, z, G, H;
    public Node(bool _isWall, int _x, int _z) { isWall = _isWall; x = _x; z = _z; }
    public int F { get { return G + H; } }
}
public class AStar : MonoBehaviour
{
    protected GameSupporter gameSupporter;
    protected Player player;
    protected Node[,] NodeArray;
    protected Node StartNode, TargetNode, CurNode;
    public List<Node> FinalNodeList;
    public List<Node> OpenList, ClosedList;

    protected bool allowDiagonal = true;
    protected bool dontCrossCorner = true;
    protected int sizeX, sizeZ;
    protected Vector3Int bottomLeft, topRight, startPos, targetPos;

    protected virtual void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        player = FindObjectOfType<Player>();
    }
    protected virtual void SetPathFinding()
    {
        // NodeArray�� ũ�� �����ְ�, isWall, x, z ����
        sizeX = topRight.x - bottomLeft.x;
        sizeZ = topRight.z - bottomLeft.z;
        NodeArray = new Node[sizeX, sizeZ];

        for (int i = 0; i < sizeX * sizeZ; i++)
        {
            bool isWall = false;
            if ((int)GameSupporter.map2DObject.wall == gameSupporter.Map2D[i / sizeZ, i % sizeZ] || (int)GameSupporter.map2DObject.moster == gameSupporter.Map2D[i / sizeZ, i % sizeZ])
            {
                isWall = true;
            }
            NodeArray[i / sizeZ, i % sizeZ] = new Node(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.z - bottomLeft.z];
    }


    protected void PathFinding(Vector3Int start, Vector3Int target, Vector3Int mapMini, Vector3Int mapMax)
    {
        startPos = start;
        
        targetPos = target;

        bottomLeft = mapMini;

        topRight = mapMax;

        SetPathFinding();

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                {
                    CurNode = OpenList[i];
                }
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // ������
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                return;
            }

            // �֢آע�
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z - 1);
                OpenListAdd(CurNode.x + 1, CurNode.z - 1);
            }

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.z + 1);
            OpenListAdd(CurNode.x + 1, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z - 1);
            OpenListAdd(CurNode.x - 1, CurNode.z);
        }
    }

    protected void OpenListAdd(int checkX, int checkZ)
    {
        if (OpenListAddCondition(checkX,checkZ))
        {
            // �밢�� ����, �� ���̷� ��� �ȵ�
            if (allowDiagonal)
            {
                if (NodeArray[CurNode.x - bottomLeft.x, checkZ - bottomLeft.z].isWall && NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall)
                {
                    return;
                }
            }

            // �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
            if (dontCrossCorner)
            {
                if (NodeArray[CurNode.x - bottomLeft.x, checkZ - bottomLeft.z].isWall || NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall)
                {
                    return;
                }
            }

            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.z - checkZ == 0 ? 10 : 14);

            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.z - TargetNode.z)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
    protected virtual bool OpenListAddCondition(int checkX, int checkZ)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z)
        {
            if (!NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]))
            {
                return true;
            }
        }
        return false;
    }
}


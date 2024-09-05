using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovement : AStar
{
    private IObjectPool<PlayerMovePlane> playerMovePlanePool;
    private GameObject playerMovePlanePrefab;
    private GameObject playerPlaneStandard;
    private List<PlayerMovePlane> playerMovePlaneList = new List<PlayerMovePlane>();
    
    private IObjectPool<SkillSelection> SkillSelectionPool;
    private GameObject SkillSelectionPrefab;
    private List<SkillSelection> skillSelectionList = new List<SkillSelection>();

    private SkillTest skillTest;
    public RaycastHit Hit { get; set; }

    // �̵��Ÿ�
    private int radiusMove = 4;

    protected override void Awake()
    {
        base.Awake();
        playerPlaneStandard = GameObject.Find("PlayerPlaneStandard");
        playerMovePlanePrefab = Resources.Load("Prefab/PlayerMovePlane", typeof(GameObject)) as GameObject;
        playerMovePlanePool = new ObjectPool<PlayerMovePlane>
            (
            CreatePlayerMovePlane,
            OnGetPlayerMovePlane,
            OnReleasePlayerMovePlane,
            OnDestroyPlayerMovePlane,
            maxSize: 500
            );

        SkillSelectionPrefab = Resources.Load("Prefab/SkillSelection", typeof(GameObject)) as GameObject;
        SkillSelectionPool = new ObjectPool<SkillSelection>
            (
            CreateSkillSelection,
            OnGetSkillSelection,
            OnReleaseSkillSelection,
            OnDestroySkillSelection,
            maxSize: 20
            );

        skillTest = GetComponent<SkillTest>();
    }
    
    public void OnClickSkillTest()
    {
        GameManager.Instance.playerState = GameManager.PlayerState.Skill;
        GameManager.Instance.skillState = skillTest;
    }

    public void Move()
    {
        GameManager.Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameManager.map2DObject.noting;

        transform.position = new Vector3(Hit.transform.position.x, transform.position.y, Hit.transform.position.z);
        playerPlaneStandard.transform.position = new Vector3(transform.position.x, 1, transform.position.z);


        GameManager.Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameManager.map2DObject.player;
    }

    // �÷��̾� �� ���� �Լ�
    //----------------------------------------------------------
    public void SetPlayerPlane()
    {
        Vector3Int adj = new Vector3Int((int)playerPlaneStandard.transform.position.x - radiusMove, 0, (int)playerPlaneStandard.transform.position.z - radiusMove);
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
                    // ���� �Ѿ�� �׿� �ش��Ѵ� ���� Map2D�� �������� �ʾ� try catch�� �ذ�
                    if (GameManager.Instance.Map2D[adj.x + X, adj.z + Z] != (int)GameManager.map2DObject.wall && GameManager.Instance.Map2D[adj.x + X, adj.z + Z] != (int)GameManager.map2DObject.moster)
                    {
                        if (Mathf.FloorToInt(Mathf.Sqrt(((X - radiusMove) * (X - radiusMove)) + (Z - radiusMove) * (Z - radiusMove))) <= radiusMove)
                        {
                            PathFinding(
                                new Vector3Int(radiusMove, 0, radiusMove),
                                new Vector3Int(X, 0, Z),
                                Vector3Int.zero,
                                new Vector3Int(radiusMove * 2 + 1, 0, radiusMove * 2 + 1)
                            );
                            if (FinalNodeList.Count > 1 && FinalNodeList.Count <= radiusMove + 1)
                            {
                                playerMovePlaneList.Add(playerMovePlanePool.Get());
                                playerMovePlaneList[playerMovePlaneList.Count - 1].transform.parent = playerPlaneStandard.transform;
                                playerMovePlaneList[playerMovePlaneList.Count - 1].transform.localPosition = new Vector3(X - radiusMove, -0.49f, Z - radiusMove);
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
    public void RemovePlayerPlane()
    {
        if (playerMovePlaneList.Count > 0)
        {
            if (playerMovePlaneList[playerMovePlaneList.Count - 1].gameObject.activeSelf == true)
            {
                for (int i = 0; i < playerMovePlaneList.Count; i++)
                {
                    if (playerMovePlaneList[i].gameObject.activeSelf == true)
                    {
                        playerMovePlaneList[i].Destroy();
                    }
                }
            }
        }
    }

    // ��ų �� ���� �Լ�
    //----------------------------------------------------------
    public void SetSkillSelection()
    {
        for (int i = 0; i < GameManager.Instance.spawnMonsters.Count; i++)
        {
            skillSelectionList.Add(SkillSelectionPool.Get());
            skillSelectionList[i].transform.position = GameManager.Instance.spawnMonsters[i].transform.position;
            skillSelectionList[i].transform.parent = GameManager.Instance.spawnMonsters[i].transform;
        }
    }
    public void RemoveSkillSelection()
    {
        if (skillSelectionList.Count > 0)
        {
            if (skillSelectionList[skillSelectionList.Count - 1].gameObject.activeSelf == true)
            {
                for (int i = 0; i < GameManager.Instance.spawnMonsters.Count; i++)
                {
                    if (skillSelectionList[i].gameObject.activeSelf == true)
                    {
                        skillSelectionList[i].Destroy();
                    }
                }
            }
        }

    }

    // Astar ���� �Լ�
    //----------------------------------------------------------
    protected override bool OpenListAddCondition(int checkX, int checkZ)
    {
        // �÷��̾ �����̴� �����¿� ������ ����� �ʰ�
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z)
        {
            // ���� ����� �ʰ�
            if ((int)transform.position.x + (checkX - radiusMove) < GameManager.Instance.MapSizeX && (int)transform.position.z + (checkZ - radiusMove) < GameManager.Instance.MapSizeZ)
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
                map2dObject = GameManager.Instance.Map2D[map2dX, map2dZ];
            }
            catch { }

            bool isWall = false;
            if ((int)GameManager.map2DObject.wall == map2dObject || (int)GameManager.map2DObject.moster == map2dObject)
            {
                isWall = true;
            }

            NodeArray[i / sizeZ, i % sizeZ] = new Node(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.z - bottomLeft.z];
    }
    
    // SkillSelection Pool ���� �Լ�
    //----------------------------------------------------------
    private SkillSelection CreateSkillSelection()
    {
        SkillSelection Selection = Instantiate(SkillSelectionPrefab).GetComponent<SkillSelection>();
        Selection.SetManagedPool(SkillSelectionPool);
        return Selection;
    }
    private void OnGetSkillSelection(SkillSelection Selection)
    {
        Selection.gameObject.SetActive(true);
    }
    private void OnReleaseSkillSelection(SkillSelection Selection)
    {
        Selection.gameObject.SetActive(false);
    }
    private void OnDestroySkillSelection(SkillSelection Selection)
    {
        Destroy(Selection.gameObject);
    }
    
    // PlayerMovePlane pool ���� �Լ�
    //----------------------------------------------------------
    private PlayerMovePlane CreatePlayerMovePlane()
    {
        PlayerMovePlane plane = Instantiate(playerMovePlanePrefab).GetComponent<PlayerMovePlane>();
        plane.SetManagedPool(playerMovePlanePool);
        return plane;
    }
    private void OnGetPlayerMovePlane(PlayerMovePlane plane)
    {
        plane.gameObject.SetActive(true);
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

using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PlayerState
    { 
        Idle,
        Move,
        Skill
    }
    public enum map2DObject
    {
        noting = 0,
        wall = 1,
        player = 2,
        moster = 3
    }

    public int MapSizeX { get; set; } = 11;
    public int MapSizeZ { get; set; } = 15;
    public int[,] Map2D { get; set; }

    public List<GameObject> spawnMonsters = new List<GameObject>();
    public Skill skillState;

    private static GameManager _instance;

    private MonsterMove monsterMove;
    private PlayerMovement playerMovement;
    private Stage1 stage1;
    private Map map;
    private RaycastHit hit;

    private bool MapCheck = true;
    private bool turnStart = false;
    private bool turnEnd = false;
    public PlayerState playerState = PlayerState.Idle;

    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
        
        monsterMove = FindAnyObjectByType<MonsterMove>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        stage1 = GetComponent<Stage1>();
        map = FindAnyObjectByType<Map>();
    }

    private void Update()
    {
        //���� Ž�� �ѹ��� �۵�
        if (MapCheck)
        {
            stage1.Opening();
            map.CheckBox();
            //miniMap.UpdateMiniMap();
            MapCheck = false;
        }
        if (turnStart)
        {
            turnStart = false;
            turnEnd = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                playerMovement.Hit = hit;
                if (hit.transform.name == "Player")
                {
                    Instance.playerState = PlayerState.Move;
                }
                else
                {
                    Instance.playerState = PlayerState.Idle;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {

        }
        if (turnEnd)
        {
            stage1.MonstersMove();
            turnEnd = false;
        }
    }
}


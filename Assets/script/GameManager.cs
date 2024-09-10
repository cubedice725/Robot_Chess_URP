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
        noting,
        wall,
        player,
        moster
    }

    public int MapSizeX { get; set; } = 11;
    public int MapSizeZ { get; set; } = 15;
    public int[,] Map2D { get; set; }

    public List<GameObject> spawnMonsters = new List<GameObject>();
    public Skill skillState;

    private static GameManager _instance;

    private Stage1 stage1;
    private Map map;

    private bool MapCheck = true;
    public bool turnStart = false;
    public bool turnEnd = false;
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
        
        if (turnEnd && Instance.playerState == PlayerState.Idle)
        {
            stage1.MonstersMove();
            turnEnd = false;
        }
    }
}


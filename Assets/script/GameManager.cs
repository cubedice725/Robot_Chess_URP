using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameManager : MonoBehaviour
{
    public enum State
    { 
        Idle,
        Move,
        Skill
    }

    private static GameManager _instance;


    private GameSupporter gameSupporter;
    private MonsterMove monsterMove;
    private PlayerMovement playerMovement;
    private MiniMap miniMap;
    private Stage1 stage1;
    private Player player;
    private Map map;
    private SkillCasting skillCasting;
    private PlayerATK playerATK;
    private RaycastHit hit;

    private bool MapCheck = true;
    private bool turnStart = false;
    private bool turnEnd = false;
    public State state = State.Idle;

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
        
        gameSupporter = GetComponent<GameSupporter>();
        monsterMove = FindAnyObjectByType<MonsterMove>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        stage1 = GetComponent<Stage1>();
        player = FindAnyObjectByType<Player>();
        map = FindAnyObjectByType<Map>();
        skillCasting = FindAnyObjectByType<SkillCasting>();
        playerATK = FindAnyObjectByType<PlayerATK>();
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
                //print(hit.transform.name);
                if (hit.transform.name == "Player")
                {
                    Instance.state = State.Move;
                }
                else if (hit.transform.name.StartsWith("SkillSelection"))
                {
                    if (gameSupporter.skillState is Skill)
                    {
                        //playerMovement.RemovePlayerPlane();
                        //playerATK.RemoveSkillSelection();
                        Instance.state = State.Skill;

                        //skillCasting.SkillGet(gameSupporter.skillState, hit.transform.parent.transform);
                        //gameSupporter.skillState = null;
                    }
                }
                else
                {
                    //playerMovement.RemovePlayerPlane();
                    //playerATK.RemoveSkillSelection();

                    //gameSupporter.skillState = null;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            print(gameSupporter.skillState);
        }
        if (turnEnd)
        {
            stage1.MonstersMove();
            turnEnd = false;
        }
    }
}


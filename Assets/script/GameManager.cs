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
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
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
        //맵을 탐색 한번만 작동
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
            // 메인 카메라를 통해 마우스 클릭한 곳의 ray 정보를 가져옴
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


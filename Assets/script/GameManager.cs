using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private GameSupporter gameSupporter;
    private MonsterMove monsterMove;
    private PlayerMove playerMove;
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

    private void Awake()
    {
        gameSupporter = GetComponent<GameSupporter>();
        monsterMove = FindAnyObjectByType<MonsterMove>();
        playerMove = FindAnyObjectByType<PlayerMove>();
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
                playerMove.Hit = hit;
                //print(hit.transform.name);
                if (hit.transform.name == "Player")
                {
                    playerMove.RemovePlayerPlane();
                    playerATK.RemoveSkillSelection();

                    playerMove.SetPlayerPlane();
                    gameSupporter.skillState = null;
                }
                else if (hit.transform.name.StartsWith("PlayerMovePlane"))
                {
                    playerMove.RemovePlayerPlane();
                    playerATK.RemoveSkillSelection();

                    playerMove.Move();
                    turnStart = true;
                }
                else if (hit.transform.name.StartsWith("SkillSelection"))
                {
                    if (gameSupporter.skillState is Skill)
                    {
                        playerMove.RemovePlayerPlane();
                        playerATK.RemoveSkillSelection();
                        skillCasting.SkillGet(gameSupporter.skillState);
                        gameSupporter.skillState = null;
                    }
                }
                else
                {
                    playerMove.RemovePlayerPlane();
                    playerATK.RemoveSkillSelection();

                    gameSupporter.skillState = null;
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


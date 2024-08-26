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
    GameSupporter gameSupporter;
    PlayerMove playerMove;
    Map map;
    MiniMap miniMap;
    MonsterMove monsterMove;
    Stage1 stage1;
    Player player;
    private bool MapCheck = true;
    protected RaycastHit hit;

    bool turnStart = false;
    bool turnEnd = false;

    private void Awake()
    {
        playerMove = FindAnyObjectByType<PlayerMove>();
        monsterMove = FindAnyObjectByType<MonsterMove>();
        player = FindAnyObjectByType<Player>();
        miniMap = FindAnyObjectByType<MiniMap>();
        map = FindAnyObjectByType<Map>();

        gameSupporter = GetComponent<GameSupporter>();
        stage1 = GetComponent<Stage1>();
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
                playerMove.Hit = hit;
                if (hit.transform.name == "Player")
                {
                    playerMove.SetPlayerPlane();
                }
                else if (hit.transform.name.StartsWith("Move Plane"))
                {
                    playerMove.Move();
                    turnStart = true;
                }
                else if (hit.transform.name.StartsWith("SkillSelection"))
                {
                    print("SkillSelection");
                }
                else
                {
                    print(hit.transform.name);
                    playerMove.RemovePlayerPlane();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            player.SetSkillSelection();
        }
        if (turnEnd)
        {
            stage1.MonstersMove();
            turnEnd = false;
        }
    }
}


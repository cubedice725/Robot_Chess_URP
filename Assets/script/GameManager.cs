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
    private bool MapCheck = true;
    protected RaycastHit hit;

    bool turnStart = false;
    bool turnEnd = false;

    private void Awake()
    {
        gameSupporter = FindAnyObjectByType<GameSupporter>();
        playerMove = FindAnyObjectByType<PlayerMove>();
        miniMap = FindAnyObjectByType<MiniMap>();
        map = FindAnyObjectByType<Map>();
        monsterMove = FindAnyObjectByType<MonsterMove>();

    }

    private void Update()
    {
        //���� Ž�� �ѹ��� �۵�
        if (MapCheck)
        {
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
                    playerMove.setPlayerPlane();
                }
                if (hit.transform.name.StartsWith("Move Plane"))
                {
                    playerMove.Move();
                    turnStart = true;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            map.CheckBox();

        }
        if (turnEnd)
        {
            monsterMove.Move();
            //miniMap.UpdateMiniMap();
            turnEnd = false;
        }
    }
}


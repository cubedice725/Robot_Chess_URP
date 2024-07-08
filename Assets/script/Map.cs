using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    protected GameSupporter gameSupporter;
    protected GameObject odbParent;
    protected GameObject odbPrefab;
    protected MiniMap miniMap;

    // odb: object Detection Box
    protected List<GameObject> odbList = new List<GameObject>();

    // 맵에 사용할 블록 생성
    private void Awake()
    {
        // 필요한 컴포넌트, 프리펩 생성
        gameSupporter = FindObjectOfType<GameSupporter>();
        miniMap = FindObjectOfType<MiniMap>();
        odbParent = GameObject.Find("Object Detection Box");
        odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        // object Detection Box 맵 크기만큼 생성
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            odbList.Add(Instantiate(odbPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), odbParent.transform));
            odbList[i].SetActive(false);
        }
        gameSupporter.Map2D = new int[gameSupporter.MapSizeX, gameSupporter.MapSizeZ];

        // 게임에 필요한 Object생성 혹은 설정
        SetCheckBox();
    }

    // CheckBox를 생성하여 확인할 준비를 함
    // 해당 함수는 필수적으로 Unity life cycle CollisionXXX 이전에 생성해야함
    public void SetCheckBox()
    {
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            odbList[i].transform.position = new Vector3(i / gameSupporter.MapSizeZ, 1, i % gameSupporter.MapSizeZ);
            odbList[i].SetActive(true);
        }
    }
    // 실제로 충돌된 값을 확인하여 map2D에 넣어줌
    public void CheckBox()
    {
        // 원위치
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].SetActive(false);
        }
    }

}

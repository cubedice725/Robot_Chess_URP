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
    // odb: object Detection Box
    private List<GameObject> odbList = new List<GameObject>();

    private GameSupporter gameSupporter;
    private GameObject odbParent;
    private GameObject odbPrefab;
    private MiniMap miniMap;

    // �ʿ� ����� ��� ����
    private void Awake()
    {
        // �ʿ��� ������Ʈ, ������ ����
        gameSupporter = FindObjectOfType<GameSupporter>();
        miniMap = FindObjectOfType<MiniMap>();
        odbParent = GameObject.Find("Object Detection Box");
        odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        // object Detection Box �� ũ�⸸ŭ ����
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            odbList.Add(Instantiate(odbPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), odbParent.transform));
            odbList[i].SetActive(false);
        }
        gameSupporter.Map2D = new int[gameSupporter.MapSizeX, gameSupporter.MapSizeZ];

        // ���ӿ� �ʿ��� Object���� Ȥ�� ����
        // CheckBox�� �����Ͽ� Ȯ���� �غ� ��
        // �ش� �Լ��� �ʼ������� Unity life cycle CollisionXXX ������ �����ؾ���
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            odbList[i].transform.position = new Vector3(i / gameSupporter.MapSizeZ, 1, i % gameSupporter.MapSizeZ);
            odbList[i].SetActive(true);
        }
    }

    // ������ �浹�� ���� Ȯ���Ͽ� map2D�� �־���
    public void CheckBox()
    {
        // ����ġ
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].SetActive(false);
        }
    }

}

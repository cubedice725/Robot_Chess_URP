using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    // odb: object Detection Box
    private IObjectPool<ObjectDetectionBox> ODBPool;

    private GameSupporter gameSupporter;
    private GameObject OdbParent;
    private GameObject OdbPrefab;
    private List<ObjectDetectionBox> OdbList = new List<ObjectDetectionBox>();

    // �ʿ� ����� ��� ����
    private void Awake()
    {
        // �ʿ��� ������Ʈ, ������ ����
        gameSupporter = FindObjectOfType<GameSupporter>();
        OdbParent = GameObject.Find("Object Detection Box");
        OdbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        // �ش� ������ �����Ͻ� ��ũ��Ʈ ���� �Һи����� ���⿡ �ۼ�
        gameSupporter.Map2D = new int[gameSupporter.MapSizeX, gameSupporter.MapSizeZ];

        ODBPool = new ObjectPool<ObjectDetectionBox>
            (
            CreateODB, 
            OnGetODB, 
            OnReleaseODB, 
            OnDestroyODB, 
            maxSize: 500
            );

        // object Detection Box �� ũ�⸸ŭ ����
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            OdbList.Add(ODBPool.Get());
            OdbList[i].transform.parent = OdbParent.transform;
        }

        // ���ӿ� �ʿ��� Object���� Ȥ�� ����
        // CheckBox�� �����Ͽ� Ȯ���� �غ� ��
        // �ش� �Լ��� �ʼ������� Unity life cycle CollisionXXX ������ �����ؾ���
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            OdbList[i].transform.position = new Vector3(i / gameSupporter.MapSizeZ, 1, i % gameSupporter.MapSizeZ);
            OdbList[i].gameObject.SetActive(true);
        }
    }

    // ������ �浹�� ���� Ȯ���Ͽ� map2D�� �־���
    public void CheckBox()
    {
        for (int i = 0; i < OdbList.Count; i++)
        {
            OdbList[i].gameObject.SetActive(false);
        }
    }
    private ObjectDetectionBox CreateODB()
    {
        ObjectDetectionBox ODB = Instantiate(OdbPrefab).GetComponent<ObjectDetectionBox>();
        ODB.SetManagedPool(ODBPool);
        return ODB;
    }
    private void OnGetODB(ObjectDetectionBox ODB)
    {
        ODB.gameObject.SetActive(false);
    }
    private void OnReleaseODB(ObjectDetectionBox ODB)
    {
        ODB.gameObject.SetActive(false);
    }
    private void OnDestroyODB(ObjectDetectionBox ODB)
    {
        Destroy(ODB.gameObject);
    }

}

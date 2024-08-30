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

    // 맵에 사용할 블록 생성
    private void Awake()
    {
        // 필요한 컴포넌트, 프리펩 생성
        gameSupporter = FindObjectOfType<GameSupporter>();
        OdbParent = GameObject.Find("Object Detection Box");
        OdbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        // 해당 변수는 컴파일시 스크립트 순서 불분명으로 여기에 작성
        gameSupporter.Map2D = new int[gameSupporter.MapSizeX, gameSupporter.MapSizeZ];

        ODBPool = new ObjectPool<ObjectDetectionBox>
            (
            CreateODB, 
            OnGetODB, 
            OnReleaseODB, 
            OnDestroyODB, 
            maxSize: 500
            );

        // object Detection Box 맵 크기만큼 생성
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            OdbList.Add(ODBPool.Get());
            OdbList[i].transform.parent = OdbParent.transform;
        }

        // 게임에 필요한 Object생성 혹은 설정
        // CheckBox를 생성하여 확인할 준비를 함
        // 해당 함수는 필수적으로 Unity life cycle CollisionXXX 이전에 생성해야함
        for (int i = 0; i < gameSupporter.MapSizeX * gameSupporter.MapSizeZ; i++)
        {
            OdbList[i].transform.position = new Vector3(i / gameSupporter.MapSizeZ, 1, i % gameSupporter.MapSizeZ);
            OdbList[i].gameObject.SetActive(true);
        }
    }

    // 실제로 충돌된 값을 확인하여 map2D에 넣어줌
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

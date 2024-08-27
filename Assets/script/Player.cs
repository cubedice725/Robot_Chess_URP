using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove), typeof(PlayerATK))]
public class Player : MonoBehaviour
{
    // 이동거리
    public int RadiusMove { get; set; } = 4;
    // 체력


    private RaycastHit Hit { get; set; }

    private List<GameObject> SkillSelectionList = new List<GameObject>();
    private GameSupporter gameSupporter;
    private int SkillSelectionSetCount = 3;
    private int SkillSelectionListCount;

    private void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        // 플래이어 판 생성
        GameObject SkillSelectionPrefab = Resources.Load("Prefab/SkillSelection", typeof(GameObject)) as GameObject;
        for (int i = 0; i < SkillSelectionSetCount; i++)
        {
            SkillSelectionList.Add(Instantiate(SkillSelectionPrefab));
            SkillSelectionList[i].transform.position = new Vector3(0, -100, 0);
            SkillSelectionList[i].SetActive(false);
        }
    }
    public void SetSkillSelection()
    {
        for( int i = 0; i < gameSupporter.spawnMonsters.Count; i++)
        {
            SkillSelectionList[i].SetActive(true);

            SkillSelectionList[i].transform.position = gameSupporter.spawnMonsters[i].transform.position;
            SkillSelectionList[i].transform.parent = gameSupporter.spawnMonsters[i].transform;
        }
    }
}
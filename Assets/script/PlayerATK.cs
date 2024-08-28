using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATK : MonoBehaviour
{
    private PlayerMove playerMove;
    private GameSupporter gameSupporter;

    GameObject SkillSelectionPrefab;
    private List<GameObject> skillSelectionList = new List<GameObject>();
    private int skillSelectionSetCount = 3;
    private int skillSelectionListCount;

    //GameObject generalSkillPrefab;
    //private List<GameObject> generalSkillList = new List<GameObject>();
    //private int generalSkillSetCount = 3;
    //private int generalSkillListCount;

    private void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        playerMove = GetComponent<PlayerMove>();

        SkillSelectionPrefab = Resources.Load("Prefab/SkillSelection", typeof(GameObject)) as GameObject;

        //generalSkillPrefab = Resources.Load("Prefab/Skill/GeneralSkills", typeof(GameObject)) as GameObject;

        for (int i = 0; i < skillSelectionSetCount; i++)
        {
            skillSelectionList.Add(Instantiate(SkillSelectionPrefab));
            skillSelectionList[i].transform.position = new Vector3(0, -100, 0);
            skillSelectionList[i].SetActive(false);
        }
    }
    public void SetSkillSelection()
    {
        skillSelectionListCount = 0;
        for (int i = 0; i < gameSupporter.spawnMonsters.Count; i++)
        {
            skillSelectionList[i].SetActive(true);

            skillSelectionList[i].transform.position = gameSupporter.spawnMonsters[i].transform.position;
            skillSelectionList[i].transform.parent = gameSupporter.spawnMonsters[i].transform;

            skillSelectionListCount++;
        }
    }
    public void RemoveSkillSelection()
    {
        if (skillSelectionListCount != 0)
        {
            for (int i = 0; i < skillSelectionListCount; i++)
            {
                skillSelectionList[i].transform.position = new Vector3(0, -100, 0);
                skillSelectionList[i].SetActive(false);
            }
        }
    }
    //public void SetGeneralSkills()
    //{
    //    skillSelectionListCount = 0;
    //    for (int i = 0; i < gameSupporter.spawnMonsters.Count; i++)
    //    {
    //        skillSelectionList[i].SetActive(true);

    //        skillSelectionList[i].transform.position = gameSupporter.spawnMonsters[i].transform.position;
    //        skillSelectionList[i].transform.parent = gameSupporter.spawnMonsters[i].transform;

    //        skillSelectionListCount++;
    //    }
    //}
    //public void RemoveGeneralSkills()
    //{
    //    if (skillSelectionListCount != 0)
    //    {
    //        for (int i = 0; i < skillSelectionListCount; i++)
    //        {
    //            skillSelectionList[i].transform.position = new Vector3(0, -100, 0);
    //            skillSelectionList[i].SetActive(false);
    //        }
    //    }
    //}
    public void OnClickGeneralSkills()
    {
        playerMove.RemovePlayerPlane();
        SetSkillSelection();
        //gameSupporter.skillState = generalSkills;
    }
}

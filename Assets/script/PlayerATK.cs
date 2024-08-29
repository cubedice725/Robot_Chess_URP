using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerATK : MonoBehaviour
{
    private PlayerMove playerMove;
    private GameSupporter gameSupporter;

    private IObjectPool<SkillSelection> SkillSelectionPool;
    private GameObject SkillSelectionPrefab;
    private List<SkillSelection> skillSelectionList = new List<SkillSelection>();

    private IObjectPool<Skill> generalSkillPool;
    private GameObject generalSkillPrefab;
    private Skill generalSkills;
    private void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        playerMove = GetComponent<PlayerMove>();

        SkillSelectionPrefab = Resources.Load("Prefab/SkillSelection", typeof(GameObject)) as GameObject;
        SkillSelectionPool = new ObjectPool<SkillSelection>(CreateSkillSelection, OnGetSkillSelection, OnReleaseSkillSelection, OnDestroySkillSelection, maxSize: 20);

        generalSkillPrefab = Resources.Load("Prefab/Skill/GeneralSkills", typeof(GameObject)) as GameObject;
        generalSkillPool = new ObjectPool<Skill>(CreateSkill, OnGetSkill, OnReleaseSkill, OnDestroySkill, maxSize: 20);
    }
    public void SetSkillSelection()
    {
        for (int i = 0; i < gameSupporter.spawnMonsters.Count; i++)
        {
            skillSelectionList.Add(SkillSelectionPool.Get());
            skillSelectionList[i].transform.position = gameSupporter.spawnMonsters[i].transform.position;
            skillSelectionList[i].transform.parent = gameSupporter.spawnMonsters[i].transform;
        }
    }
    public void RemoveSkillSelection()
    {
        for (int i = 0; i < gameSupporter.spawnMonsters.Count; i++)
        {
            skillSelectionList[i].Destroy();
        }
    }
    
    public void OnClickGeneralSkills()
    {
        playerMove.RemovePlayerPlane();
        SetSkillSelection();
        generalSkills = generalSkillPool.Get();
        gameSupporter.skillState = generalSkills;
    }

    // Skill Pool 관련 함수
    //-----------------------------------------------------------
    private Skill CreateSkill()
    {
        Skill skill = Instantiate(generalSkillPrefab).GetComponent<Skill>();
        skill.SetManagedPool(generalSkillPool);
        return skill;
    }
    private void OnGetSkill(Skill skill)
    {
        skill.gameObject.SetActive(false);
    }
    private void OnReleaseSkill(Skill skill)
    {
        skill.gameObject.SetActive(false);
    }
    private void OnDestroySkill(Skill skill)
    {
        Destroy(skill.gameObject);
    }

    // SkillSelection Pool 관련 함수
    //-----------------------------------------------------------
    private SkillSelection CreateSkillSelection()
    {
        SkillSelection Selection = Instantiate(SkillSelectionPrefab).GetComponent<SkillSelection>();
        Selection.SetManagedPool(SkillSelectionPool);
        return Selection;
    }
    private void OnGetSkillSelection(SkillSelection Selection)
    {
        Selection.gameObject.SetActive(true);
    }
    private void OnReleaseSkillSelection(SkillSelection Selection)
    {
        Selection.gameObject.SetActive(false);
    }
    private void OnDestroySkillSelection(SkillSelection Selection)
    {
        Destroy(Selection.gameObject);
    }
}

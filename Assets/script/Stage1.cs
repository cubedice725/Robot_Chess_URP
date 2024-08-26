using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    GameSupporter gameSupporter;
    GameObject monster;

    private void Awake()
    {
        gameSupporter = FindAnyObjectByType<GameSupporter>();
        monster = Resources.Load("Prefab/Monster/Monster", typeof(GameObject)) as GameObject;
    }
    public void Opening()
    {
        gameSupporter.spawnMonsters.Add(Instantiate(monster, new Vector3(7, 0.9f, 13), Quaternion.Euler(Vector3.zero)));
        gameSupporter.spawnMonsters.Add(Instantiate(monster, new Vector3(3, 0.9f, 13), Quaternion.Euler(Vector3.zero)));
    }

    public void MonstersMove()
    {
        gameSupporter.spawnMonsters[0].GetComponent<MonsterMove>().Move();
        gameSupporter.spawnMonsters[1].GetComponent<MonsterMove>().Move();
    }
}

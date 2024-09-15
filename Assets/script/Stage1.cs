using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    GameObject monster;

    private void Awake()
    {
        monster = Resources.Load("Prefab/Monster/MonsterBasic", typeof(GameObject)) as GameObject;
    }
    public void Opening()
    {
        GameManager.Instance.spawnMonsters.Add(Instantiate(monster, new Vector3(7, 0.9f, 13), Quaternion.Euler(Vector3.zero)));
        GameManager.Instance.spawnMonsters.Add(Instantiate(monster, new Vector3(3, 0.9f, 13), Quaternion.Euler(Vector3.zero)));
    }

    public void Update()
    {
        for (int i = 0; i < GameManager.Instance.spawnMonsters.Count; i++) 
        {
            if (GameManager.Instance.spawnMonsters[i].GetComponent<Monster>().flag)
            {

            }
            else
            {
                return;
            }
        }
        GameManager.Instance.monsterTurn = false;
    }
}

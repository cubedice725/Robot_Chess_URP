using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterBasicMovingState))]
[RequireComponent(typeof(MonsterBasicIdleState))]
[RequireComponent(typeof(MonsterBasicSkillCastingState))]
[RequireComponent(typeof(MonsterBasicMovement))]
public class MonsterBasic : Monster
{
    public bool start = true;
    public int myNum;
    
    public override void UpdateMonster()
    {
        if (GameManager.Instance.monsterTurn)
        {
            myNum = FindMyNum();
            if (myNum == 0)
            {
                if (start)
                {
                    if (monsterMovement.AttackNavigation())
                    {
                        monsterState = MonsterState.Skill;
                    }
                    else
                    {
                        monsterState = MonsterState.Move;
                    }
                    start = false;
                }
            }
            else if (GameManager.Instance.spawnMonsters[myNum - 1].GetComponent<Monster>().flag == true)
            {
                if (start)
                {
                    if (monsterMovement.AttackNavigation())
                    {
                        monsterState = MonsterState.Skill;
                    }
                    else
                    {
                        monsterState = MonsterState.Move;
                    }
                    start = false;
                }
            }
        }
        else
        {
            start = true;
        }
    }
    public int FindMyNum()
    {
        int num;
        for (int i = 0; i < GameManager.Instance.spawnMonsters.Count; i++)
        {
            if (GameManager.Instance.spawnMonsters[i].GetComponent<Monster>() == this)
            {
                return num = i;
            }
        }
        return 0;
    }
}

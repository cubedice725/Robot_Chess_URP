using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicSkillCastingState : MonsterSkillCastingState
{
    public override void Enter()
    {
        monsterMovement.Attack();
    }
    public override void IStateUpdate()
    {
        monster.flag = true;
    }
    public override void Exit()
    {
        monster.flag = false;
    }
}
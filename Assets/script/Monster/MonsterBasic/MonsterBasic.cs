using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterBasicMovingState))]
[RequireComponent(typeof(MonsterBasicIdleState))]
[RequireComponent(typeof(MonsterBasicSkillCastingState))]
[RequireComponent(typeof(MonsterBasicMovement))]
public class MonsterBasic : Monster
{
    public override void UpdateMonster()
    {
        if (Input.GetMouseButtonDown(1))
        {
            monsterState = MonsterState.Move;
        }
    }
}

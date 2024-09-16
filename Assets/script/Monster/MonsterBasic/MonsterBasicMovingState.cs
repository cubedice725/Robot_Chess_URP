using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicMovingState : MonsterMovingState
{
    public override void Enter()
    {
    }
    public override void IStateUpdate()
    {
        if (!monsterMovement.Move())
        {
            monster.monsterState = Monster.MonsterState.Idle;
        }
    }
    public override void Exit()
    {

    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicMovingState : MonsterMovingState
{
    public override void Enter()
    {
        monsterMovement.Move();
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

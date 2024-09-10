using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicMovingState : MonsterMovingState
{
    public override void Enter()
    {
        print("들어옴");
    }
    public override void IStateUpdate()
    {
        print("성공적으로 움직임");
    }
    public override void Exit()
    {
        print("잘 나감");
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicMovingState : MonsterMovingState
{
    public override void Enter()
    {
        print("����");
    }
    public override void IStateUpdate()
    {
        print("���������� ������");
    }
    public override void Exit()
    {
        print("�� ����");
    }

    

}

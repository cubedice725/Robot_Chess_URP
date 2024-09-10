using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterMovingState : MonoBehaviour, IState
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void IStateUpdate();
}

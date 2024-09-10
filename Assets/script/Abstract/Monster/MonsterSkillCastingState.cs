using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSkillCastingState : MonoBehaviour, IState
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void IStateUpdate();
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class MonsterSkillCastingState : MonoBehaviour, IState
{
    public Monster monster;
    public MonsterMovement monsterMovement;
    public virtual void Awake()
    {
        monster = GetComponent<Monster>();
        monsterMovement = GetComponent<MonsterMovement>();
    }
    public abstract void Enter();
    public abstract void IStateUpdate();
    public abstract void Exit();
}

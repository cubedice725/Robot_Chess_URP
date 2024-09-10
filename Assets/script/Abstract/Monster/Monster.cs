using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MonsterStateMachine))]
public abstract class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        Idle,
        Move,
        Skill
    }
    public MonsterState monsterState = MonsterState.Idle;
    public MonsterStateMachine monsterStateMachine;
    public MonsterMovement monsterMovement;
    public void Awake()
    {
        monsterMovement = GetComponent<MonsterMovement>();
        monsterStateMachine = GetComponent<MonsterStateMachine>();
        monsterStateMachine.Initialize(monsterStateMachine.monsterIdleState);
    }
    public void Update()
    {
        if (monsterState == MonsterState.Idle)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.monsterIdleState);
        }
        else if (monsterState == MonsterState.Move)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.monsterMovingState);
        }
        else if (monsterState == MonsterState.Skill)
        {
            monsterStateMachine.TransitionTo(monsterStateMachine.monsterSkillCastingState);
        }
        monsterStateMachine.PlayerStateMachineUpdate();
        UpdateMonster();
    }
    public abstract void UpdateMonster();
}
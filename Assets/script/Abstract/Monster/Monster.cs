using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    // 몬스터의 
    public bool flag = false;
    public enum MonsterState
    {
        Idle,
        Move,
        Skill
    }
    public MonsterState monsterState = MonsterState.Idle;
    public MonsterStateMachine monsterStateMachine;
    public MonsterMovement monsterMovement;
    public virtual void Awake()
    {
        monsterMovement = GetComponent<MonsterMovement>();
        monsterStateMachine = GetComponent<MonsterStateMachine>();
        monsterStateMachine.Initialize(monsterStateMachine.monsterIdleState);
    }
    public void Update()
    {
        //몬스터 턴과 상관없이 움직임
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
        
        monsterStateMachine.MonsterStateMachineUpdate();
        
        // 몬스터 턴인 경우 개발자가 작성하여 몬스터 움직임을 설정
        UpdateMonster();

        ////몬스터 턴이 아닌경우 움직임
        if (!GameManager.Instance.monsterTurn)
        {
            monsterState = MonsterState.Idle;
        }
    }
    public abstract void UpdateMonster();
}
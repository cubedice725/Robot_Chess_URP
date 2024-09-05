using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerSkillCastingState : IState
{
    private PlayerMovement _PlayerMovement;
    private RaycastHit hit;

    public PlayerSkillCastingState(PlayerMovement playerMovement)
    {
        _PlayerMovement = playerMovement;
    }
    public void Enter()
    {
        GameManager.Instance.skillState.Enter();
    }
    public void IStateUpdate()
    {
        GameManager.Instance.skillState.IStateUpdate();
    }
    public void Exit()
    {
        GameManager.Instance.skillState.Exit();
    }
}

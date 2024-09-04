using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerStateMachine = new PlayerStateMachine(playerMovement);
        playerStateMachine.Initialize(playerStateMachine.playerIdleState);

    }
    private void Update()
    {
        if (GameManager.Instance.state == GameManager.State.Idle) 
        {
            playerStateMachine.TransitionTo(playerStateMachine.playerIdleState);
        }
        else if (GameManager.Instance.state == GameManager.State.Move)
        {
            playerStateMachine.TransitionTo(playerStateMachine.playerMovingState);
        }
        else if (GameManager.Instance.state == GameManager.State.Skill)
        {
            playerStateMachine.TransitionTo(playerStateMachine.playerSkillCastingState);
        }
        playerStateMachine.Update();
    }
}
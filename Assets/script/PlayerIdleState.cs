using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerMovement playerMovement;
    public PlayerIdleState(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }
    public void Enter()
    {

    }
    public void Update()
    {

    }
    public void Exit()
    {
        
    }
}

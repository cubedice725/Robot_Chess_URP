using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerMovement _playerMovement;
    public PlayerIdleState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    public void Enter()
    {

    }
    public void IStateUpdate()
    {

    }
    public void Exit()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine
{
    public IState CurrentState {  get; private set; }

    public PlayerSkillCastingState playerSkillCastingState;
    public PlayerMovingState playerMovingState;
    public PlayerIdleState playerIdleState;
    
    public PlayerStateMachine(PlayerMovement player)
    {
        playerSkillCastingState = new PlayerSkillCastingState(player);
        playerMovingState = new PlayerMovingState(player);
        playerIdleState = new PlayerIdleState(player);
    }
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }
    public void TransitionTo(IState nextState)
    {
        if (nextState == CurrentState)
            return;
        
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

    }
    public void Update()
    {
        if (CurrentState != null) 
        {
            CurrentState.Update();
        }
    }
}

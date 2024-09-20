using UnityEngine;
using static GameManager;
public class PlayerSkillCastingState : IState
{
    private RaycastHit hit;

    public void Enter()
    {
        Instance.skillState.Enter();
    }
    public void IStateUpdate()
    {
        Instance.skillState.IStateUpdate();
    }
    public void Exit()
    {
        Instance.skillState.Exit();
        Instance.skillState = null;
    }
}

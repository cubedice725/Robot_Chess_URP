using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerMovement _playerMovement;
    private RaycastHit hit;

    public PlayerIdleState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    public void Enter()
    {

    }
    public void IStateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.name == "Player")
                {
                    GameManager.Instance.playerState = GameManager.PlayerState.Move;
                }
            }
        }
    }
    public void Exit()
    {
        
    }
}

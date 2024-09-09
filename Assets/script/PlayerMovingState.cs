using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerMovingState : IState
{
    private PlayerMovement _playerMovement;
    private RaycastHit hit;

    public PlayerMovingState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    public void Enter()
    {
        _playerMovement.SetPlayerPlane();
    }
    public void IStateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 메인 카메라를 통해 마우스 클릭한 곳의 ray 정보를 가져옴
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                _playerMovement.Hit = hit;

                if (hit.transform.name.StartsWith("PlayerMovePlane"))
                {
                    _playerMovement.RemovePlayerPlane();
                    _playerMovement.Move();
                    GameManager.Instance.playerState = GameManager.PlayerState.Idle;
                }
            }
        }
    }
    public void Exit()
    {
        _playerMovement.RemovePlayerPlane();
    }
}

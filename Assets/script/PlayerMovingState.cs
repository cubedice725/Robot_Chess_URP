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
            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
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

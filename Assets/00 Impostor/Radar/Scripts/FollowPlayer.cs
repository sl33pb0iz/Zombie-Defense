using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class FollowPlayer : MonoBehaviour
{
    void LateUpdate()
    {
        if (GameManager.Instance.GameStateController.CurrentGameState != GameState.IN_GAME)
            return;

        if (PlayerStateMachine.Instance)
            transform.position = PlayerStateMachine.Instance.transform.position + Vector3.up * 10;
    }
}

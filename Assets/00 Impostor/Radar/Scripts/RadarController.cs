using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Unicorn.Examples;

public class RadarController : MonoBehaviour
{

    [SerializeField] private RectTransform PlayerVisionRadar;

    private void Update()
    {
        // Lấy hướng nhìn của người chơi (ví dụ: hướng của trục Z)
        Vector3 playerForward = PlayerStateMachine.Instance.transform.forward;

        // Tính toán góc alpha từ hướng nhìn của người chơi
        float alpha = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;

        // Xoay RectTransform
        PlayerVisionRadar.localRotation = Quaternion.Euler(0f, 0f, -alpha);
    }
}

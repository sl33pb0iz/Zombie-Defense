using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Sirenix.OdinInspector;

public class Frozenable : MonoBehaviour
{
    public ParticleSystem OnFrozen;

    private bool _IsFrozenig;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private CapsuleCollider[] m_capsuleColliders;


    public Health Health { get; private set; }

    void Awake()
    {
        Health = GetComponent<Health>();
        if (!Health)
        {
            Health = GetComponentInParent<Health>();
        }
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>(); 

    }

    private void OnDisable()
    {
        OnFrozen.Stop();
        StopAllCoroutines();
    }

    public void InflictStartFrozening(float damage, float frozenTime,GameObject damageSource)
    {
        if (_IsFrozenig)
        {
            // Nếu enemy đang trong trạng thái đóng băng, không thực hiện gì
            return;
        }

        // Gây sát thương cho enemy
        Health.TakeDamage(damage, damageSource);

        // Bắt đầu đóng băng
        StartCoroutine(StartFrozening(frozenTime));
    }

    [System.Obsolete]
    private IEnumerator StartFrozening(float FrozenDuration)
    {
        _IsFrozenig = true;

        // Lưu lại velocity hiện tại của rigidbody
        Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;

        // Dừng rigidbody bằng cách đặt velocity thành Vector3.zero
        m_Rigidbody.velocity = Vector3.zero;

        // Lưu lại trạng thái hoạt ảnh hiện tại của enemy
        bool currentAnimationState = m_Animator.enabled;

        // Tạm dừng hoạt ảnh của enemy bằng cách vô hiệu hóa Animator
        m_Animator.enabled = false;

        // Bật particle effect khi enemy bị đóng băng
        OnFrozen.Play();

        OnFrozen.startLifetime = FrozenDuration;

        // Đóng băng enemy trong một khoảng thời gian
        yield return new WaitForSeconds(FrozenDuration);

        // Dừng particle effect
        OnFrozen.Stop();

        // Khôi phục velocity ban đầu của rigidbody
        m_Rigidbody.velocity = currentVelocity;

        // Khôi phục trạng thái hoạt ảnh ban đầu của enemy
        m_Animator.enabled = currentAnimationState;

        // Tiếp tục di chuyển enemy
        _IsFrozenig = false;
    }
}

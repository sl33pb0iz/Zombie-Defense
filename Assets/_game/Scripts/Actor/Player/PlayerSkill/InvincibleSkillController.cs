using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class InvincibleSkillController : ActivatedSkill
{
    public float invincibleTime;
    private Health playerHealth => PlayerStateMachine.Instance.m_health;

    public override void CastSkill()
    {
        StartCoroutine(playerHealth.IEInvincibleTime(invincibleTime));
    }

}

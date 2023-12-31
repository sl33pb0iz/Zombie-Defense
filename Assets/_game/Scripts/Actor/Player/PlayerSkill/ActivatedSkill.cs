using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spicyy.System;

public abstract class ActivatedSkill : MonoBehaviour
{
    public float radiusAroundPlayer;
    public PlayerCastSkillEvent.ActivatedSkillType skillType;
    public abstract void CastSkill();

}

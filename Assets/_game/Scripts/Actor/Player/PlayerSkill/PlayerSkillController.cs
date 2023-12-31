using System.Collections.Generic;
using UnityEngine;
using Spicyy.System;

public class PlayerSkillController : MonoBehaviour
{
    public List<ActivatedSkill> m_ListSkills; 
    private readonly Dictionary<PlayerCastSkillEvent.ActivatedSkillType, ActivatedSkill> m_Skills = new Dictionary<PlayerCastSkillEvent.ActivatedSkillType, ActivatedSkill>();

    private void Start()
    {
        foreach(var skills in m_ListSkills)
        {
            m_Skills.Add(skills.skillType, skills);
        }
    }

    private void OnEnable()
    {
        EventManager.AddListener<PlayerCastSkillEvent>(OnPlayerCastSkill);        
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PlayerCastSkillEvent>(OnPlayerCastSkill);
    } 

    void OnPlayerCastSkill(PlayerCastSkillEvent evt)
    {
        if (m_Skills.TryGetValue(evt.skillType, out ActivatedSkill skillController))
        {
            skillController.CastSkill();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class MinimapIconManager : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Actor actor;
    public Health actorHealth;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (actorHealth)
        {
            actorHealth.OnDie += Character_OnDie;
        }
        ShowEnemy();
    }

    private void OnDisable()
    {
        if (actorHealth)
        {
            actorHealth.OnDie -= Character_OnDie;
        }
        HideEnemy();
    }

    private void Character_OnDie()
    {
        sprite.color = Color.clear;
    }

    private void ShowEnemy()
    {
        if (actor.isPlayer)
        {
            sprite.color = Color.blue;
            transform.localScale = Vector3.one * 8f;
        }
        //else if (actor.Affiliation != PlayerStateMachine.Instance.m_Actor.Affiliation)
        //{
        //    sprite.color = Color.red;
        //}
        //else if(actor.Affiliation == PlayerStateMachine.Instance.m_Actor.Affiliation)
        //{
        //    sprite.color = Color.green;
        //}    
    }

    private void HideEnemy()
    {

            sprite.color = Color.clear;
        
    }
}

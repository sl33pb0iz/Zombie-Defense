using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using Unicorn.UI;
using UnityEngine.SceneManagement;
using Spicyy.System;

public class BunkerManager : MonoBehaviour
{
    public List<FloorController> m_FloorsManager = new List<FloorController>();
    public BunkerUIManager m_BunkerUI;

    private PlayerDataManager playerDataManager => PlayerDataManager.Instance;
    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        //playerStateMachine.OnAwake();

        for(int index = 0; index <= m_FloorsManager.Count; index++)
        {
            //if(m_FloorsManager[index] != playerDataManager.GetCu)
        }
    }

    private void Start()
    {
        EventManager.Broadcast(Events.StartLevelEvent);
    }

    //private void Update()
    //{
    //    //playerStateMachine.OnUpdate();
    //}

    private void AddFloor(FloorController floor)
    {
        m_FloorsManager.Add(floor);
    }

    private void RemoveFloor(FloorController floor)
    {
        m_FloorsManager.Remove(floor);
    } 
}

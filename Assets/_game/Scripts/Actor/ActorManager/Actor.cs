using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Actor : MonoBehaviour
    {
        [Tooltip("Represents the affiliation (or team) of the actor. Actors of the same affiliation are friendly to each other")]
        public int Affiliation;
        public bool isPlayer = false; 

        ActorsManager m_ActorsManager;

        public void Start()
        {
            m_ActorsManager = FindObjectOfType<ActorsManager>();
            if (!m_ActorsManager.Actors.Contains(this))
            {
                m_ActorsManager.Actors.Add(this);
            }
        }
        void OnDisable()
        {
            // Unregister as an actor
            if (m_ActorsManager)
            {
                m_ActorsManager.Actors.Remove(this);
            }
        }
    }
}

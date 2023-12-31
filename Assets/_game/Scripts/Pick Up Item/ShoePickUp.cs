using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ShoePickUp : PickUp
    {
        [Header("Parameters")]
        [Tooltip("Amount of health to heal on pickup")]
        public float speedUpAmount; 

        protected override void OnPicked(PlayerStateMachine player)
        {
            
        }
    }
}

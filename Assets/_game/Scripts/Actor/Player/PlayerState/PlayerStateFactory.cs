using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerStateFactory 
    {
        private PlayerStateMachine _context;
        public PlayerStateFactory (PlayerStateMachine currentContext)
        {
            _context = currentContext;
        }

        public PlayerBaseState Attack()
        {
            return new PlayerAttackState(_context, this);
        }

        public PlayerBaseState Defend()
        {
            return new PlayerDefendState(_context, this);
        }

        public PlayerBaseState Die()
        {
            return new PlayerDieState(_context, this);
        }

        public PlayerBaseState Walk()
        {
            return new PlayerWalkState(_context, this);
        }

        public PlayerBaseState Shoot()
        {
            return new PlayerShootState(_context, this);
        }

        public PlayerBaseState Build()
        {
            return new PlayerBuildState(_context, this);
        }

    }
}

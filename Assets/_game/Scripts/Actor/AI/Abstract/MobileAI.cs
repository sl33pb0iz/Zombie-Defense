using Sirenix.OdinInspector;
using UnityEngine;

namespace Spicyy.AI
{
    public abstract class MobileAI : AI
    {
        [Title("MOVERMENT", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] protected float rotationSpeed = 360f;
        [SerializeField] protected float movingSpeed = 10f;

        protected abstract void Move(Transform target);
        protected abstract void Die();

    }
}



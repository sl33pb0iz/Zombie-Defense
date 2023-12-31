using UnityEngine;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Tự động huỷ object sau một thời gian.
    /// </summary>
    public class AutoDestructor : MonoBehaviour
    {
        enum TypeDestroy
        {
            Disable,
            PutToPool,
            Destroy
        }

        [SerializeField] private float timeDestroy = 1.5f;
        [SerializeField] private TypeDestroy typeDestroy;

        private void OnEnable()
        {
            Invoke(nameof(AutoDestroy), timeDestroy);
        }

        private void AutoDestroy()
        {
            switch (typeDestroy)
            {
                case TypeDestroy.Disable:
                    gameObject.SetActive(false);
                    break;
                case TypeDestroy.PutToPool:
                    SimplePool.Despawn(gameObject);
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}
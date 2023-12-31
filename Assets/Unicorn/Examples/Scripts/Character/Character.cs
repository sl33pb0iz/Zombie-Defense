using UnityEngine;
using Sirenix.OdinInspector;

namespace Unicorn.Examples
{
    public class Character : MonoBehaviour
    {
        private PlayerSkinChanger skinCharacter;

        public PlayerSkinChanger SkinCharacter
        {
            get => skinCharacter;
            protected set => skinCharacter = value;
        }

        [field: SerializeField]
        public bool IsPlayer { get; set; }

        protected virtual void Awake()
        {
            skinCharacter = GetComponent<PlayerSkinChanger>();
            Init();
        }

        public void Init()
        {
            skinCharacter.Init();
        }
    }
}
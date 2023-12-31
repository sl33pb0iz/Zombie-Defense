using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


namespace Unicorn.Examples
{
    [RequireComponent(typeof(UnityEngine.CharacterController))]
    public class Pet : MonoBehaviour
    {
        [SerializeField] private float minRestTime = 1;
        [SerializeField] private float maxRestTime = 3;
        [Range(0, 1)] [SerializeField] private float actionRatio = 0.3f;
        [SerializeField] private float speed = 10;
        [SerializeField] private float rotationSpeed = 360;
        [SerializeField] private float squaredStoppingDistance = 0.3f;
        [SerializeField] private float randomRadius = 25;
        [SerializeField] private Animator animator;
        private UnityEngine.CharacterController controller;

        private Vector3 target;
        private Quaternion targetRotation;
        private static readonly int RunHashed = Animator.StringToHash("Run");

        private float nextMoveTime;

        private Character owner;
        private Vector3 ownerOffset;
        private float maxDistanceFromOwner;

        private float actionFinishTime;
        private static readonly int Action = Animator.StringToHash("Action");

        public bool CanMove { get; set; } = true;

        public Animator Animator => animator;

        public static Pet Spawn(Character character, Vector3 ownerOffset, float maxDistanceFromOwner, int petId = -1)
        {
            var prefabs = GameManager.Instance.PlayerDataManager.DataTextureSkin.pets;
            if (petId == -1)
            {
                petId = Random.Range(0, prefabs.Length);
            }

            var prefab = prefabs[petId];
            var startDistance = Random.Range(1, maxDistanceFromOwner);
            var pet = Instantiate(
                prefab,
                character.transform.position + ownerOffset,
                Quaternion.identity);

            SceneManager.MoveGameObjectToScene(pet.gameObject, LevelManager.Instance.gameObject.scene);

            pet.RegisterOwner(character, ownerOffset, maxDistanceFromOwner);
            pet.transform.localScale = character.transform.lossyScale;

            return pet;
        }

        public static Pet Spawn(int petId = -1)
        {
            var prefabs = GameManager.Instance.PlayerDataManager.DataTextureSkin.pets;
            if (petId == -1)
            {
                petId = Random.Range(0, prefabs.Length);
            }

            var prefab = prefabs[petId];
            var pet = Instantiate(prefab);

            SceneManager.MoveGameObjectToScene(pet.gameObject, LevelManager.Instance.gameObject.scene);
            return pet;
        }

        private void Awake()
        {
            if (!animator)
            {
                animator = GetComponentInChildren<Animator>();
            }

            controller = GetComponent<UnityEngine.CharacterController>();
            target = transform.position;
        }

        private void Start()
        {
            var renderer = GetComponentInChildren<Renderer>();
            controller.center = renderer.bounds.size.y / 2 * Vector3.up;
            controller.height = renderer.bounds.size.y;
            if (controller.stepOffset > controller.height)
            {
                controller.stepOffset = controller.height * transform.lossyScale.magnitude;
            }
        }

        public void RegisterOwner(Character character, Vector3 ownerOffset, float maxDistanceFromOwner)
        {
            owner = character;
            this.ownerOffset = ownerOffset;
            this.maxDistanceFromOwner = maxDistanceFromOwner;
        }

        public void RemoveOwner()
        {
            owner = null;
        }

        private void Update()
        {
            if (!CanMove) return;

            float sqrTargetDistanceToOwner = owner ? (target - owner.transform.position).sqrMagnitude : 0;

            if (owner && sqrTargetDistanceToOwner >= maxDistanceFromOwner * maxDistanceFromOwner)
            {
                FindNewPosition();
                nextMoveTime = Time.time;
            }

            UpdateSpeed();
            if (nextMoveTime > Time.time)
            {
                StopMoving();
                DoAction();
                return;
            }
            
            var distance = (target - transform.position.Set(y: target.y)).sqrMagnitude;
            if (distance < squaredStoppingDistance)
            {
                FindNewPosition();
                return;
            }

            MoveToTarget();
        }

        private void UpdateSpeed()
        {
            if (!owner)
            {
                speed = 10;
                return;
            }

            float sqrDistanceToOwner = (transform.position - owner.transform.position).sqrMagnitude;
            speed = Mathf.Min(2 + sqrDistanceToOwner / 5f, 20);
        }

        private void StopMoving()
        {
            animator.SetBool(RunHashed, false);
        }

        private void DoAction()
        {
            if (actionFinishTime > Time.time) return;

            var rand = Random.value;
            if (rand > actionRatio)
            {
                actionFinishTime = Time.time + 0.3f;
                return;
            }

            animator.SetTrigger(Action);
            actionFinishTime = Time.time + 4;

            nextMoveTime = Mathf.Max(Time.time + 1f, nextMoveTime);
        }

        private void FindNewPosition()
        {
            animator.SetBool(RunHashed, false);
            nextMoveTime = Time.time + Random.Range(minRestTime, maxRestTime);

            if (!owner)
            {
                FindRandomPosition();
            }
            else
            {
                FindPositionAroundOwner();
            }
        }

        private void FindRandomPosition()
        {
            var direction = Random.insideUnitCircle.ToVectorXZ().normalized;
            direction *= Random.Range(0, 2) == 0 ? -1 : 1;

            target = transform.position + direction * Random.Range(2, randomRadius);
            targetRotation = Quaternion.LookRotation(target - transform.position);
        }

        private void FindPositionAroundOwner()
        {
            var direction = Random.insideUnitCircle.ToVectorXZ().normalized;
            direction *= Random.Range(0, 2) == 0 ? -1 : 1;

            target = owner.transform.position + direction * Random.Range(2, maxDistanceFromOwner) + ownerOffset;
            targetRotation = Quaternion.LookRotation(target - transform.position);
        }


        private void MoveToTarget()
        {
            animator.SetBool(RunHashed, true);

            var newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            var motion = newPosition - transform.position;

            motion += Physics.gravity * Time.deltaTime;
            controller.Move(motion);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Pet")) return;
            if (hit.point.y < transform.position.y) return;
            target = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, randomRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target, 1);
        }
    }

}
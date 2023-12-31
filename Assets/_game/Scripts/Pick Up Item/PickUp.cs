using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using Spicyy.System;

namespace Unicorn
{
    public class PickUp : MonoBehaviour
    {

        [Tooltip("VFX spawned on pickup")] public GameObject PickupVfxPrefab;

        bool m_HasPlayedFeedback;

        private Collider m_Collider;

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            m_Collider.enabled = true;
            ItemsLevelManager.Instance.RegisterItem(this);
            EventManager.AddListener<LevelWinEvent>(OnLevelWin);
            EventManager.AddListener<NewStageStartEvent>(OnNewStage);
            Drop();  
        }

        private void OnDisable()
        {
            ItemsLevelManager.Instance.UnRegisterItem(this);
            EventManager.RemoveListener<LevelWinEvent>(OnLevelWin);
            EventManager.RemoveListener<NewStageStartEvent>(OnNewStage);

        }

        void OnLevelWin(LevelWinEvent evt) => StartCoroutine(MoveToTarget(PlayerStateMachine.Instance.transform, Random.Range(0.5f, 1f)));
        void OnNewStage(NewStageStartEvent evt) => StartCoroutine(MoveToTarget(PlayerStateMachine.Instance.transform, Random.Range(0.5f, 1f)));

        IEnumerator MoveToTarget(Transform targetPosition, float duration)
        {
            m_Collider.enabled = false;

            while((transform.position - targetPosition.position).sqrMagnitude >= 6f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition.position, 0.03f);
                yield return null;
            }

            transform.position = targetPosition.position;
            yield return null;

            OnPicked(PlayerStateMachine.Instance);
        }

        private void Drop()
        {
            float randomJumpForce = Random.Range(1, 10);
            var randomDuration = Random.Range(0.5f, 1.5f);
            var randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out var hit, 30, NavMesh.AllAreas))
            {
                transform.DOJump(hit.position + new Vector3(0, 1.5f, 0), randomJumpForce, 1, randomDuration);
            }
            else if (NavMesh.SamplePosition(transform.position, out var hit1, 30, NavMesh.AllAreas))
            {
                transform.DOJump(hit1.position + new Vector3(0, 1.5f, 0), randomJumpForce, 1, randomDuration);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
        protected virtual void OnPicked(PlayerStateMachine playerController)
        {
            PlayPickupFeedback(playerController);
        }

        public void PlayPickupFeedback(PlayerStateMachine playerController)
        {
            if (m_HasPlayedFeedback)
                return;
            if (PickupVfxPrefab)
            {
                var pickupVfxInstance = PoolManager.Instance.ReuseObject(PickupVfxPrefab, transform.position, Quaternion.identity);
                pickupVfxInstance.SetActive(true);
            }
            m_HasPlayedFeedback = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerStateMachine.Instance.gameObject)
            {
                OnPicked(PlayerStateMachine.Instance);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using UnityEngine.AI;

public class LightningSkillController : ActivatedSkill
{

    public Lightning stormFX;
    public float duration = 5f;
    public int lightingPerWave = 3;
    public float delayBetweenBolts = 1f;

    public override void CastSkill()
    {
        StartCoroutine(GenerateLightning());
        IEnumerator GenerateLightning()
        {

            float startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                for (int boltIndex = 0; boltIndex < lightingPerWave; boltIndex++)
                {
                    Vector3 randomPosition = GetRandomPointOnNavMesh();
                    GameObject stormLightningFX = PoolManager.Instance.ReuseObject(stormFX.gameObject, randomPosition, stormFX.transform.rotation);
                    SoundManager.Instance.PlayFxSound(stormFX.m_Sound);
                    stormLightningFX.SetActive(true);
                }
                yield return new WaitForSeconds(delayBetweenBolts);
            }

        }
    }
    protected  Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = Vector3.zero;
        NavMeshHit hit;

        while (true)
        {
            // Generate a random point inside a sphere around the player
            Vector3 randomOffset = Random.insideUnitSphere * radiusAroundPlayer;
            Vector3 randomPosition = this.transform.position + randomOffset;

            // Sample the position to find the nearest point on the NavMesh
            if (NavMesh.SamplePosition(randomPosition, out hit, radiusAroundPlayer, NavMesh.AllAreas))
            {
                randomPoint = hit.position;
                break;
            }
        }

        return randomPoint;
    }
}

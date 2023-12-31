using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unicorn;
using Sirenix.OdinInspector; 

[System.Serializable]
public class RocketSkillController : ActivatedSkill
{
    [FoldoutGroup("SET UP")]public GameObject rocketPrefab;
    [FoldoutGroup("SET UP")]public GameObject targetAimingVFX;

    [FoldoutGroup("PARAMETER")] public float FXCounter;
    [FoldoutGroup("PARAMETER")] public int rocketPerWave;
    [FoldoutGroup("PARAMETER")] public int numwaves;
    [FoldoutGroup("PARAMETER")] public float waveInterval;
    [FoldoutGroup("PARAMETER")] public float rocketSpeed;
    [FoldoutGroup("PARAMETER")] public float rocketHeight;
    [FoldoutGroup("PARAMETER")] public float rocketDamage;

    public override void CastSkill()
    {
        StartCoroutine(SpawnRocket());
        IEnumerator SpawnRocket()
        {
            for (int wave = 0; wave < numwaves; wave++)
            {
                for (int index = 0; index < rocketPerWave; index++)
                {
                    Vector3 rocketPosition = GetRandomPointOnNavMesh();

                    // Kiểm tra xem vị trí ngẫu nhiên có nằm trong vùng tầm nhìn của camera hay không
                    GameObject rocket = PoolManager.Instance.ReuseObject(rocketPrefab, rocketPosition + new Vector3(0, rocketHeight, 0), Quaternion.identity);
                    GameObject targetVFX = PoolManager.Instance.ReuseObject(targetAimingVFX, rocketPosition, targetAimingVFX.transform.rotation);
                    targetVFX.SetActive(true);
                    targetAimingVFX.SetActive(true);
                    rocket.transform.forward = Vector3.down;
                    ProjectileStandard prjRocket = rocket.GetComponent<ProjectileStandard>();
                    prjRocket.Damage = rocketDamage;
                    prjRocket.m_Velocity = rocket.transform.forward * rocketSpeed;
                    rocket.SetActive(true);
                    StartCoroutine(IEFXOff());

                    IEnumerator IEFXOff()
                    {
                        yield return Yielders.Get(FXCounter);
                        targetVFX.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(waveInterval);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TornadoSkillController : ActivatedSkill
{

    public GameObject twistPrefab;
    public float twistCounter;
    
    public override void CastSkill()
    {
        for (int index = 0; index < twistCounter; index++)
        {
            Vector3 tornadoPosition = GetRandomPointOnNavMesh();
            GameObject twist = PoolManager.Instance.ReuseObject(twistPrefab, tornadoPosition, twistPrefab.transform.rotation);
            twist.SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSetRandomMesh : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    [SerializeField] private List<Mesh> m_RandomMesh;

    private void OnEnable()
    {
        int randomIndex = Random.Range(0, m_RandomMesh.Count);
        ChangeMesh(randomIndex);
    }

    public void ChangeMesh(int index)
    {
        m_SkinnedMeshRenderer.sharedMesh = m_RandomMesh[index];
    }
}

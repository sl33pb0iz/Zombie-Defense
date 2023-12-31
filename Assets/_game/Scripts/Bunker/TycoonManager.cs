using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;

public class TycoonManager : MonoBehaviour
{
    private static TycoonManager instance;
    public static TycoonManager Instance => instance;


    protected virtual void Awake()
    {
        instance = this;
    }

    protected virtual void Start()
    {
        GameManager.Instance.RegisterTycoonScene(this);
    }

    public virtual void OnUpdate()
    {

    }

    protected virtual void OnDestroy() { }
}

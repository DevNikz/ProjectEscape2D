using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private List<Transform> targetList;
    [SerializeField] private List<GameObject> boundList;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private bool tryMoving;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        TrySetTarget(1);

        boundList = GameObject.FindGameObjectsWithTag("Bounds").ToList();
    }

    public void TryEnableBounds()
    {
        if(boundList != null) EnableAllBounds();
    }

    public void TryDisableBounds()
    {
        if(boundList != null) DisableAllBounds();
    }

    void EnableAllBounds()
    {
        for(int i = 0; i < boundList.Count; i++)
        {
            boundList[i].gameObject.SetActive(true);
        }
    }

    void DisableAllBounds()
    {
        for(int i = 0; i < boundList.Count; i++)
        {
            boundList[i].gameObject.SetActive(false);
        }
    }

    public void TrySetTarget(int value)
    {
        if(targetList != null && value > 0) SetCurrentTarget(value-1);
    }

    public void SetCurrentTarget(int index)
    {
        currentTarget = targetList[index];
    }

    public Transform GetCurrentTarget() { return currentTarget; }


    public void SetMoving(bool value) { tryMoving = value; }
    public bool HasMoved() { return tryMoving; }
}

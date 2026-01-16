using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class LockObject : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if(animator != null)
        {
            animator.Play("Lock");
        }
    }
}

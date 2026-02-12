using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 10f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        ApplyRandomForce();
    }

    void ApplyRandomForce()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float forceMagnitude = Random.Range(minForce, maxForce);
        rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
    }
}

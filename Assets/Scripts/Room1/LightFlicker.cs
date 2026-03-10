using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0f;
    public float maxIntensity = 1f;
    public float flickerSpeed = 0.1f; // Time between intensity changes

    private Light2D lightSource;
    private float timeUntilNextFlicker;

    void Start()
    {
        lightSource = GetComponent<Light2D>();
        timeUntilNextFlicker = flickerSpeed;
    }

    void Update()
    {
        // Update timer
        timeUntilNextFlicker -= Time.deltaTime;

        if (timeUntilNextFlicker <= 0)
        {
            // Change intensity to a random value within the specified range
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            // Reset timer with a random delay for more natural flicker
            timeUntilNextFlicker = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f); 
        }
    }
}

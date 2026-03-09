using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCollider : MonoBehaviour
{

    [SerializeField] bool detected;

    public bool GetDetect() { return detected; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("LightBulb"))
        {
            //Debug.Log("Detected");
            detected = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log($"{other.name} | {other.tag}");
        if(other.CompareTag("LightBulb"))
        {
            GetComponent<Light2D>().intensity = 0.5f;
            detected = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("LightBulb"))
        {
            GetComponent<Light2D>().intensity = 0.01f;
            detected = false;
        }
    }
}

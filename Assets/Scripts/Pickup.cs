using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float throwMultiplier = 2f;  // how strong the throw is
    [SerializeField] private float maxThrowForce = 15f;   // cap force
    // public float maxDistance = 0.5f;
    // [SerializeField] private float frequency = 8f;
    // [SerializeField] private float damping = 1f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private HingeJoint2D currentJoint;
    [SerializeField] private Rigidbody2D grabbedBody;
    private Vector2 lastMousePos;
    private Vector2 mouseVelocity;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        // Track mouse velocity
        mouseVelocity = (mousePos - lastMousePos) / Time.deltaTime;
        lastMousePos = mousePos;

        if (InputManager.Instance.IsMouseButtonDownThisFrame()) TryGrab(mousePos);
        if (InputManager.Instance.IsMouseButtonUpThisFrame()) Release();
        if (currentJoint != null) currentJoint.connectedAnchor = mousePos;        
    }

    void TryGrab(Vector2 mousePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(mousePos, pickupLayer);

        if (hit != null && hit.attachedRigidbody != null)
        {
            grabbedBody = hit.attachedRigidbody;

            Cursor.lockState = CursorLockMode.Confined;

            // Add hinge joint to the OBJECT (not camera)
            currentJoint = grabbedBody.gameObject.AddComponent<HingeJoint2D>();
            currentJoint.autoConfigureConnectedAnchor = false;

            // Cursor acts like an invisible anchor
            currentJoint.connectedBody = null; // world space
            currentJoint.anchor = grabbedBody.transform.InverseTransformPoint(mousePos);
            currentJoint.connectedAnchor = mousePos;

            // Joint behavior
            currentJoint.useLimits = true;
            currentJoint.limits = new JointAngleLimits2D { min = -180, max = 180 };
            currentJoint.useMotor = false;
        }
    }

    void Release()
    {
        // Apply throw force
        Vector2 throwForce = mouseVelocity * throwMultiplier;
        throwForce = Vector2.ClampMagnitude(throwForce, maxThrowForce);
        grabbedBody.linearVelocity = throwForce;

        Cursor.lockState = CursorLockMode.None;
        grabbedBody = null;

        if(currentJoint != null) {
            Destroy(currentJoint); 
        }
    }
}

/*
public class MousePickup : MonoBehaviour
{
    public LayerMask pickupLayer;
    public float dragForce = 50f;
    public float damping = 5f;

    private SpringJoint2D joint;
    private Rigidbody2D grabbedBody;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            TryGrab(mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }

        if (joint != null)
        {
            joint.connectedAnchor = mousePos;
        }
    }

    void TryGrab(Vector2 mousePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(mousePos, pickupLayer);

        if (hit != null)
        {
            grabbedBody = hit.GetComponent<Rigidbody2D>();

            if (grabbedBody != null)
            {
                joint = gameObject.AddComponent<SpringJoint2D>();
                joint.autoConfigureDistance = false;
                joint.distance = 0f;
                joint.dampingRatio = 1f;
                joint.frequency = dragForce;
                joint.connectedBody = grabbedBody;
            }
        }
    }

    void Release()
    {
        if (joint != null)
            Destroy(joint);

        grabbedBody = null;
    }
}
*/
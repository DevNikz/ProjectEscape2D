using UnityEngine;

public abstract class BasePhysicsItem : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D connectedObject;
    protected Vector2 clickPos;

    public virtual void SetClickPosition()
    {
        clickPos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetMouseScreenPosition());
    }

    public virtual void OnMouseDown()
    {
        if(connectedObject != null)
        {
            HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
            joint.connectedBody = connectedObject;
            SetClickPosition();
            joint.autoConfigureConnectedAnchor = true;
        }
    }
}

/*
public class JointCreatorPrecise : MonoBehaviour
{
    public Rigidbody2D connectedObject;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        if (connectedObject != null)
        {
            HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
            joint.connectedBody = connectedObject;

            // Calculate the click point in world coordinates
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Set the joint's anchor to the click position relative to the Rigidbody's center of mass
            // Auto-configure the connected anchor, as it's easier to manage in most cases.
            joint.autoConfigureConnectedAnchor = true;
            
            // To make the joint anchor point precise to where the mouse clicked on the *this* object,
            // you could set the anchor and connected anchor manually, but `autoConfigureConnectedAnchor` 
            // simplifies this significantly. A simpler alternative to a Hinge Joint for
            // a 'pickup' behavior might be a TargetJoint2D, which connects to a specified target
            // in world space.
        }
    }
}
*/
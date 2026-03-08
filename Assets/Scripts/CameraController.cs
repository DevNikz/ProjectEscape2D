using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCameraBase cam;
    private CinemachinePositionComposer posComp;
    private CinemachineBrain brain;
    public List<Transform> targetList; // Assign your player object in the Inspector
    public Transform SelectedTarget;
    public float smoothSpeed = 0.125f;
    Vector2 velocity = new Vector2(1, 0);
    public Vector3 offset; // Adjust the camera offset in the Inspector (e.g., (0, 0, -10) for 2D)
    int currentIndex = 0;

    public Vector3 referencePosition;
    public float distance;

    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCameraBase>();
        posComp = cam.GetComponent<CinemachinePositionComposer>();
    }

    void Update()
    {
        //HandleInput();
    }

    void FixedUpdate()
    {
        if(LevelManager.Instance.HasMoved() == true) SwitchScene();
    }

    void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % targetList.Count;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + targetList.Count) % targetList.Count;
        }
    }

    void SwitchScene()
    {
        //SelectedTarget = targetList[currentIndex];
        SelectedTarget = LevelManager.Instance.GetCurrentTarget();

        //Disable Bounds
        LevelManager.Instance.TryDisableBounds();

        //Move
        cam.Follow = SelectedTarget;
        posComp.TargetOffset = offset;

        Vector2 desiredPos = SelectedTarget.position + transform.position;
        Vector2 smoothed = Vector2.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        //Vector2 smoothed = Vector2.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        transform.position = smoothed;

        distance = Vector2.Distance(transform.position, desiredPos / 2);

        if(distance < 0.5f) {
            LevelManager.Instance.TryEnableBounds();
            LevelManager.Instance.SetMoving(false);
        }
    }

}

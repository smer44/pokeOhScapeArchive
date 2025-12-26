using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivotTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform characterTransform;
    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 200f; // degrees per second at Mouse X/Y of 1
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private bool invertY = false;

    [Header("UX")]
    [SerializeField] private bool lockCursor = true;

    [Header("Modes")]
    [SerializeField] private bool thirdPersonMode = true; 
    [SerializeField] public Transform firstPersonTransformForPosition;

    private float cameraPitch = 0f; // X
    private float cameraYaw = 0f; // Y
    private Vector3 thirdPersonTransformPositionBackup;

    public bool IsThirdPerson()
    {
        return thirdPersonMode;
    }

    public Transform GetCameraPivotTransform()
    {
        return cameraPivotTransform;
    }

    void Start()
    {
        thirdPersonTransformPositionBackup = cameraTransform.transform.localPosition;
        Debug.Log ("thirdPersonTransformPositionBackup : thirdPersonTransformPositionBackup");
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;


        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float sign = invertY ? 1f : -1f; // natural look usually uses negative

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleMode();
        }


        if (thirdPersonMode)
        {
            cameraPitch += mouseY * mouseSensitivity * delta * sign;
            cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);


            cameraYaw += mouseX * mouseSensitivity * delta;


            cameraPivotTransform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        }
        else
        {
            //actually the same, moved the functionality to state manager
            cameraPitch += mouseY * mouseSensitivity * delta * sign;
            cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
            //cameraPivotTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

            cameraYaw += mouseX * mouseSensitivity * delta;
            cameraPivotTransform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        }

    }

    public void ToggleMode()
    {
        thirdPersonMode = !thirdPersonMode;
        if (thirdPersonMode)
        {
            
            cameraTransform.transform.localPosition = thirdPersonTransformPositionBackup;

        }
        else
        {
            cameraTransform.transform.localPosition = Vector3.zero;
        }
        Debug.Log($"setting localPosition to : { this.transform.localPosition}");
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!lockCursor) return;


        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Optionally set yaw/pitch at runtime (in degrees). Pitch will be clamped.
    /// </summary>
    public void SetYawPitch(float yaw, float pitch)
    {
        cameraYaw = yaw;
        cameraPitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        if (cameraPivotTransform != null)
        {
            cameraPivotTransform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        }
    }


}

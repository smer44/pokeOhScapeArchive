using System.Collections;

using Unity.VisualScripting;

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager Instance { get; private set; }

    private bool isInitialZoomOut = true;
    public float rotationSpeed = 5f;
    private Vector3 initialPos;
    [SerializeField] private float camHeight = 2;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    // Camera scroll stuff
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float minZoom = -2f;
    [SerializeField] private float maxZoom = 20f;


    [SerializeField] private float noMoveTimeDelay = 5.0f;
    private float lastMoveTime = -1.0f;
    [SerializeField] private Transform cam;
    [SerializeField] private float camDistance;
    private float targetCamDistance;
    [SerializeField] private Transform pivotTarget;
    private bool isCamLerping = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos = transform.position;

        if (Instance == null)
        {
            Instance = this;
        }

        targetCamDistance = camDistance;
        camDistance = 0.0f;
        //camDistance = cam.localPosition.z * -1f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = pivotTarget.position;
        if (isInitialZoomOut)
        {
            camDistance += Time.deltaTime * 2.0f;
            if (camDistance >= targetCamDistance)
            {
                camDistance = targetCamDistance;
                isInitialZoomOut = false;
            }
            cam.localPosition = new Vector3(0, camHeight, -camDistance);
        }
        else
        {
            if (!isCamLerping)
            {
                Vector3 movePos = new Vector3(0, 0, 0);
                if (Input.GetKey(KeyCode.W))
                {
                    movePos += transform.forward;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    movePos -= transform.forward;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    movePos -= transform.right;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    movePos += transform.right;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    movePos += transform.up;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    movePos -= transform.up;
                }
                transform.localPosition += movePos * Time.deltaTime * 5.0f;
            }

            if (Input.GetMouseButton(2))
                {
                    UpdateByMouseInput();
                }

            cameraZoom();
        }

    }

    void UpdateByMouseInput()
    {
        // Get mouse horizontal and vertical movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        float currPitch = transform.eulerAngles.x;
        float currYw = transform.eulerAngles.y;

        // Calculate rotation around the Y axis (horizontal)
        currYw += mouseX * rotationSpeed;
        currPitch -= mouseY * rotationSpeed;

        // Clamping yaw 
        currPitch = Mathf.Clamp(currPitch, minPitch, maxPitch);

        // Rotating
        transform.localRotation = Quaternion.Euler(currPitch, currYw, 0);

    }

    void cameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        camDistance -= scrollSpeed * scroll;
        camDistance = Mathf.Clamp(camDistance, minZoom, maxZoom);

        cam.localPosition = new Vector3(0, camHeight, -camDistance);

    }

    public void CameraMove(Vector3 pos)
    {
        StopAllCoroutines();
        StartCoroutine (LerpToPosition(pos));

    }

    [SerializeField] private float moveDuration = 2.5f; // drag-and-edit in Unity

    private IEnumerator LerpToPosition(Vector3 targetPosition)
    {
        isCamLerping = true;
        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(start, targetPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isCamLerping = false;
    }
}
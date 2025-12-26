using UnityEngine;

public class ObjectToMoveCamera : MonoBehaviour
{

    public CameraManager cameraManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnMouseDown()
    {
        cameraManager.CameraMove(this.transform.position);
    }
}

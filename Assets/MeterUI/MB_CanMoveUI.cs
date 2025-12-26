using UnityEngine;
using UnityEngine.UI;

public class MB_CanMoveUI : MonoBehaviour
{
    public UnitMB unit;
    [SerializeField] private Image panel;
    [SerializeField] private bool isFacingCamera = false;

    private Camera mainCamera;

    void Start()
    {
        // Caching the camera. Change the cached value if we plan on moving the camera around.
        mainCamera = Camera.main;

        if (!panel)
        {
            Debug.Log($"{gameObject.name}'s panel is null reference.");
        }

        if (unit)
        {
            unit.onCanActChanged += (GameObject obj, bool isUnitCanAct) =>
            {
                panel.color = isUnitCanAct ? Color.green : Color.grey;
            };

            panel.color = unit.CanAct ? Color.green : Color.grey;
        }

    }


    void Update()
    {
        // Make the meter always face the camera.
        if (isFacingCamera)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);            
        }
    }
}

using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created{
    public float rotationSpeed = 5.0f; // Speed multiplier for rotation
    //public BattleFieldMB battleFieldMB;
    public bool adjustCenter = true;


    void Start()
    {
        if (adjustCenter)
        {
            SetCenterByBattleFieldSize();
        }
        

    }

    void Update()
    {
        if (Input.GetMouseButton(2)) 
            UpdateRotationInput();
    }

    void UpdateRotationInput()
    {
        // Get horizontal mouse movement
        float mouseX = Input.GetAxis("Mouse X");

        // Calculate the rotation amount
        float rotationAmount = mouseX * rotationSpeed;

        // Apply rotation around the Y axis (horizontal rotation)
        transform.Rotate(0f, rotationAmount, 0f);
    }

    void SetCenterByBattleFieldSize()
    {
        BattleFieldMB battleFieldMB = BattleFieldMB.Instance;
        float centerX = (battleFieldMB.width-1f) / 2f;
        float centerZ = (battleFieldMB.height-1f) / 2f;
        transform.position = new Vector3(centerX, transform.position.y, centerZ);            
    }
}


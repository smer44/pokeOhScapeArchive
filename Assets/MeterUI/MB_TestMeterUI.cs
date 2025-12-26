using UnityEngine;

public class MB_TestMeterUI : MonoBehaviour
{
    [SerializeField] private MB_MeterUI meter;
    [SerializeField] private float loopTime = 5;
    [SerializeField] private float updateValue = 0.1f;

    private float lastUpdateTime = -1;
    private float value = 1;
    private bool isGoingDown = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastUpdateTime > loopTime)
        {
            lastUpdateTime = Time.time;

            if (value <= 0)
            {
                isGoingDown = false;
            }
            else if (value >= 1)
            {
                isGoingDown = true;
            }

            if (isGoingDown)
            {
                value -= updateValue;
            }
            else
            {
                value += updateValue;
            }

            meter?.UpdateMeterValue(value);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class MB_MeterUI : MonoBehaviour
{
    [SerializeField] UnitMB unit;
    [SerializeField] StatType attributeToFollow;

    [SerializeField] private Image gradiant;
    [SerializeField] private Image foreground;
    [SerializeField] private float stallTime = 0.1f;
    [SerializeField] private float lerpFactor = 2f;
    [SerializeField] private bool isFacingCamera = false;

    private float targetValue = 1;
    private float currentValue = 1;
    private float updateTime = -1;
    private Camera mainCamera;

    void Start()
    {
        // Caching the camera. Change the cached value if we plan on moving the camera around.
        mainCamera = Camera.main;

        if (gradiant)
        {
            gradiant.fillAmount = 1;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}'s meter UI has no gradiant specified.");
        }

        if (foreground)
        {
            foreground.fillAmount = 1;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}'s meter UI has no foreground specified.");
        }

        if (unit)
        {
            //Stat logStat = unit.data.GetStat(attributeName);
            //Debug.Log($"MB_MeterUI.Updating :for GameObject {this.gameObject}:

            Action<StatType, int> newOnAction = (StatType attributeName, int newValue) =>
            {
                //Debug.Log($"MB_MeterUI.Updating :for GameObject {this.gameObject}: stat {attributeName} , value {newValue}");
                Stat stat = unit.data.GetStat(attributeName);
                //t.data.GetStat(attributeName, out stat);
                int maxValue = stat.maxValue;
                if (maxValue != 0) {

                    UpdateMeterValue( (float)newValue / (float) maxValue);
                }
            };
            unit.data.AddOnAttributeChanged(attributeToFollow, newOnAction);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}'s unit refernce is null.");
        }
    }



    void Update()
    {
        // Make the meter always face the camera.
        if (isFacingCamera)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);            
        }

        // gradiant animation code below.
        if (gradiant == null || foreground == null)
        {
            return;
        }

        if (targetValue > currentValue)
        {
            gradiant.fillAmount = targetValue;
            if (Time.time - updateTime > stallTime)
            {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * lerpFactor);
                foreground.fillAmount = currentValue;
            }
        }
        else if (targetValue < currentValue)
        {
            foreground.fillAmount = targetValue;
            if (Time.time - updateTime > stallTime)
            {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * lerpFactor);
                gradiant.fillAmount = currentValue;
            }
        }
    }

    /// <summary>
    /// Update the meter value and does the gradiant animation for the meter. (NOTE: all values must be normalized to 0 - 1)
    /// </summary>
    /// <param name="newValue"> Normalized new value (will be clamped to 0 - 1 internally.)</param>
    public void UpdateMeterValue(float newValue)
    {
        //Debug.Log($"UpdateMeterValue ")
        targetValue = Mathf.Clamp01(newValue);
        updateTime = Time.time;
    }
}

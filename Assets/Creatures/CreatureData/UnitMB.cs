using System;
using UnityEngine;
using System.Collections.Generic;

public class UnitMB : MonoBehaviour
{
    //#TODO we need attribute variable without using strings.
    //[Tooltip("Returns attributeName, oldValue, newValue, maxValue on any attribute that was changed.")]
    //public Action<string, float, float, float> onAttributeChanged;

    public Action<GameObject, bool> onCanActChanged;

    public UnitSO initialData;
    public UnitSO data;
    public StatUpdateList statUpdateList;

    //public HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();

    public List<SOAbstractStatusEffect> statusEffects;

    public UnitDisposition disposition;
    public string team;
    [SerializeField] protected bool canAct = true;
    public bool CanAct
    {
        get => canAct;
        set
        {
            if (canAct != value)
            {
                canAct = value;
                onCanActChanged?.Invoke(this.gameObject, value);
            }
        }
    }




    public bool IsAlive()
    {
        return data.GetHP() > 0;
    }

    void Start()
    {
        ResetData();
    }

    public void SetData(UnitSO inputData)
    {
        initialData = inputData;
        ResetData();
    }

    void ResetData()
    {

        data = initialData.Clone();

        //    data.onAttributeChanged += (string attributeName, float oldValue, float newValue, float maxValue) =>
        //    {
        //        onAttributeChanged?.Invoke(attributeName, oldValue, newValue, maxValue);
        //    };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExecuteCoroutine<T>(IEnumerator<T> func)
    {
        StartCoroutine(func);
    }

    public bool IsFlying()
    {
        foreach (SOAbstractStatusEffect effect in statusEffects)
        {
            if (effect is SOFlyingStatusEffect)
            {
                return true;
            }
        }
        return false;
    }

    public void AddStatusEffect(SOAbstractStatusEffect effect)
    {
        Debug.Log($"AddStatusEffect: unit: {this},  effect : {effect}");
        statusEffects.Add(effect);
        effect.OnStart(this.gameObject);
    }
    
    
}

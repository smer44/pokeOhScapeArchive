using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatListSO", menuName = "UnitData/StatListSO")]
public class StatListSO : ScriptableObject
{
    [Header("Stats Info:")]

    [SerializeField]

    public Stat[] stats;

    public StatListSO Clone()
    {
        StatListSO clone = ScriptableObject.CreateInstance(this.GetType()) as StatListSO;
        clone.stats = new Stat[stats.Length];

        for (int i = 0; i < stats.Length; i++)
        {
            Stat? oldStat = stats[i];
            if (oldStat != null)
            {
                Stat existingStat = (Stat)oldStat;
                clone.stats[i] = CloneStat(existingStat);
            }
        }

        return clone;
    }

    public Stat CloneStat(Stat stat)
    {
        Stat clone = new Stat();
        clone.onValueChange = (Action<StatType, int>)stat.onValueChange?.Clone();
        clone.type = stat.type;
        clone.value = stat.value;
        clone.minValue = stat.minValue;
        clone.maxValue = stat.maxValue;
        return clone;
    }

    public static bool SetValue(Stat stat, int newValue)
    {

        int oldValue = stat.value;
        newValue = Mathf.Clamp(newValue, stat.minValue, stat.maxValue);
        if (oldValue != newValue)
        {
            //bool isNull = stat.onValueChange == null;
            //Debug.Log($"SetValue:for stat {stat}: newValue {newValue} , stat.onValueChange.isNull : {isNull}");
            stat.value = newValue;
            stat.onValueChange?.Invoke(stat.type, newValue);
            //onValueChanges[(int)stat.type]?.Invoke(stat.type, newValue);
            return true;
        }
        return false;
    }

    public bool ChangeValue(Stat stat, int diff)
    {
        int newValue = stat.value + diff;
        return SetValue(stat, newValue);
    }


    public bool ChangeValue(StatType statType, int diff)
    {
        Stat stat = GetStat(statType);
        return ChangeValue(stat, diff);
    }

    public bool SetValue(StatType statType, int newValue)
    {
        Stat stat = GetStat(statType);
        return SetValue(stat, newValue);
    }

    public int GetValue(StatType statType)
    {
        Stat stat = GetStat(statType);
        return stat.value;
    }

    public Stat GetStat(StatType statType)
    {
        foreach (Stat knownStat in stats) {
            if (knownStat.type == statType)
            {
                return knownStat;

            }
            
        }
        throw new System.Exception($"StatListSO.GetStat|{this.ToString()}|: unknown stat {statType}");
        
    }

    public void AddOnAttributeChanged(StatType statType, Action<StatType, int> addedAction)
    {

        Stat stat = GetStat(statType);
        stat.onValueChange += addedAction;
        //Debug.Log($"AddOnAttributeChanged: {this}: statType {statType} , stat { stat}, addedAction {addedAction} , stat.onValueChange : |{stat.onValueChange}|");





    }
}

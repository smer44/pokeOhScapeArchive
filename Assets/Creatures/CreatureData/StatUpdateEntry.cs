using UnityEngine;

[System.Serializable]
public class StatUpdateEntry
{
    public StatType type;
    public StatUpdateVariant variant;

    public void UpdateStats(UnitSO initialData, UnitSO data)
    {
        Stat initialStat = initialData.GetStat(type);
        Stat dataStat = data.GetStat(type);
        switch (variant)
        {
            case StatUpdateVariant.Value: StatListSO.SetValue(dataStat, initialStat.value);
                break;
            default:
                throw new System.Exception($"StatUpdateEntry.UpdateStats: initialData {initialData} ,data {data}, unsupported  StatUpdateVariant {variant}");
        }
        
    }

}

using UnityEngine;

[System.Serializable]
public class StatChangeEntry 
{
    public StatType type;
    public StatUpdateVariant variant;

    public int changeValue;

    public void UpdateStat(UnitSO data)
    {
        switch (variant)
        {

            case StatUpdateVariant.Value:
                data.ChangeValue(type, changeValue);
                break;
            default:
                throw new System.Exception($"StatChangeEntry.UpdateStat: initialData {data}, unsupported  StatUpdateVariant {variant}");
            
        }

    }
}

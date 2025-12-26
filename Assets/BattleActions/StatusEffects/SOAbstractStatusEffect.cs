using UnityEngine;

//[CreateAssetMenu(fileName = "SOAbstractStatusEffect", menuName = "SOBattleActions/SOAbstractStatusEffect")]
public abstract class SOAbstractStatusEffect : ScriptableObject
{
    public int priority { get; private set; }
    public int maxTurnsToLive;
    public int currentTurnsToLive;

    public int power;

    public string name;

    public abstract void OnTurnStart(GameObject unit);
    public abstract void OnTurnEnd(GameObject unit);

    public abstract void OnStart(GameObject unit);
    public abstract void OnEnd(GameObject unit);

    public abstract void OnBattleStart(GameObject unit);

    public SOAbstractStatusEffect Clone()
    {
        SOAbstractStatusEffect clone = ScriptableObject.CreateInstance(this.GetType()) as SOAbstractStatusEffect;
        clone.name = name + "_clone";
        clone.priority = priority;
        clone.power = power;
        clone.maxTurnsToLive = maxTurnsToLive;
        clone.currentTurnsToLive = maxTurnsToLive;
        return clone;
    }

    public void TickOnTurnEnd()
    {
        currentTurnsToLive -= 1;
    }

    public bool Ends()
    {
        if (currentTurnsToLive == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }




}

using UnityEngine;

public enum BattleActionTargetType
{
    FriendlyUnit, // Target is an allied unit
    EnemyUnit,    // Target is an enemy unit
    FreeCell      // Target is an empty cell on the battlefield
}


public enum BattleActionTargetQuantity
{
    None,       // No target
    Single,     // One target
    Two,        // Two targets
    Three,      // Three targets
    Four,       // Four targets
    Five,       // Five targets
    Six,        // Six targets
    All         // All possible targets
}




[CreateAssetMenu(fileName = "BattleActionUnitRequirenment", menuName = "Scriptable Objects/BattleActionUnitRequirenment")]
public class BattleActionUnitRequirenment : ScriptableObject
{
    public string name;
    public BattleActionTargetType targetType;
    //use -1 for "all"
    public int min;
    public int max;


    /// <summary>
    /// Returnst true if we need to select more tarvets for given requirenment.
    /// It is true if min is set to "all" or we having already less then min we need.
    /// However if we already selected max possible units on the battlefield, we 
    /// do not need more targets
    /// </summary>
    /// <param name="alreadyHaving"></param>
    /// <param name="maxPossible"></param>
    /// <returns></returns>
    public bool NeedMoreTargets(int alreadyHaving, int maxPossible)
    {
        return min == -1 ||  alreadyHaving < min;

    }

    /// <summary>
    /// Returnst true if we can  select more tarvets for given requirenment.
    /// It is true if max is set to "all" or we having already less then max we need.
    /// However if we already selected max possible units on the battlefield, we 
    /// do not need more targets
    /// </summary>
    /// <param name="alreadyHaving"></param>
    /// <param name="maxPossible"></param>
    /// <returns></returns>
    public bool CanHaveMoreTargets(int alreadyHaving, int maxPossible)
    {
 

        return max == -1 || alreadyHaving < max;
    }

}

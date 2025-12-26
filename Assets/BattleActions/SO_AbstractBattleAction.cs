using UnityEngine;
using System.Collections.Generic;



//[CreateAssetMenu(fileName = "AbstractBattleActionSO", menuName = "SOBattleActions/BattleActionMenuItemSingleSO")]
public abstract class AbstractBattleActionSO : ScriptableObject
{
    public string[] path;
    public string name;
    public int manaCost;
    public abstract bool IsValidFirstPerformer(BattleFieldMB field, GameObject unit);

    /// <summary>
    /// Makes check after selection first performer if this action 
    /// is generally available in the battlefield, what
    /// influence on if is this action active in selection menu or not
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public abstract bool CanAct(BattleFieldMB field);
    public abstract void Act(BattleFieldMB field);

    /// <summary>
    /// checks, if currently it is possible to finish selection
    /// also adding auto-selected unit to the selection list if nessesary
    /// </summary>
    /// <param name="field"></param>

    public abstract bool CanFinishSelection(BattleFieldMB field);

    public abstract bool IsValidTarget(BattleFieldMB field, GameObject unit);

    /// <summary>
    /// Is called when you select current action from the menu for given battlefield 
    /// </summary>
    /// <param name="battleFieldMB"></param>
    /// 
    //TODO move it to BattleFieldMB


    public abstract List<GameObject> GetPossibleTargets(BattleFieldMB field, GameObject performer);


    




}

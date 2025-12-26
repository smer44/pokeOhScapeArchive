using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


public class BattleFieldMemory
{
    public BattleFieldState fieldState;
    public GameObject selectedFirstPerformer;
    public HashSet<GameObject> selectedTargets;
    public AbstractBattleActionSO selectedAction;

    public BattleFieldMemory()
    {
        this.fieldState = BattleFieldState.SelectingFirstPerformer;
        this.selectedFirstPerformer = null;
        this.selectedTargets = new HashSet<GameObject>();
        this.selectedAction = null;
    }

    public void Reset()
    {
        this.selectedAction = null;
        this.selectedFirstPerformer = null;
        this.fieldState = BattleFieldState.SelectingFirstPerformer;
        this.selectedTargets.Clear();

    }

    public BattleFieldMemory Clone()
    {
        BattleFieldMemory ret = new BattleFieldMemory();
        ret.selectedAction = this.selectedAction;
        ret.selectedFirstPerformer = this.selectedFirstPerformer;
        ret.fieldState = this.fieldState;
        ret.selectedTargets = new HashSet<GameObject>(this.selectedTargets);
        return ret;
    }

    public override string ToString()
    {
        string performerName = selectedFirstPerformer != null ? selectedFirstPerformer.name : "None";
        string actionName = selectedAction != null ? selectedAction.name : "None";
        string targets = selectedTargets.Count > 0 ? string.Join(", ", selectedTargets.Select(t => t.name)) : "None";

        return $"State: {fieldState}, Performer: {performerName}, Action: {actionName}, Targets: [{targets}]";
    }

}

public class BattleFieldMemoryStack
{
    public Stack<BattleFieldMemory> mem;
    public Action<BattleFieldState> onBattleFieldStateChanged;

    public BattleFieldMemoryStack()
    {
        mem = new Stack<BattleFieldMemory>();
        var first = new BattleFieldMemory();
        mem.Push(first);
    }
    /*
        public void MemorizeAction(AbstractBattleActionSO action)
        {
            var last = mem.Peek().Clone();
            last.selectedAction = action;
            mem.Push(last);

        }
    */

   public void MemorizeFieldState(BattleFieldState state)
    {
        var last = mem.Peek().Clone();
        last.fieldState = state;
        mem.Push(last);

    }

/*     public void MemorizeFirstPerformer(GameObject unit)
    {
        var last = mem.Peek().Clone();
        last.selectedFirstPerformer = unit;
        mem.Push(last);
    }
*/
    public void MemorizeFirstPerformerWithFieldState(GameObject unit, BattleFieldState state)
    {
        var last = mem.Peek().Clone();
        last.selectedFirstPerformer = unit;
        last.fieldState = state;
        mem.Push(last);
        onBattleFieldStateChanged?.Invoke(last.fieldState);
    }

    public void MemorizeActionWithFieldState(AbstractBattleActionSO action, BattleFieldState state)
    {
        var last = mem.Peek().Clone();
        last.selectedAction = action;
        last.fieldState = state;
        mem.Push(last);
        onBattleFieldStateChanged?.Invoke(last.fieldState);
    }



    public void MemorizeTarget(GameObject target)
    {
        var last = mem.Peek().Clone();
        last.selectedTargets.Add(target);
        mem.Push(last);
        onBattleFieldStateChanged?.Invoke(last.fieldState);
    }

    public void SetCurrentState(BattleFieldState state)
    {
        var last = mem.Peek();
        last.fieldState = state;
        onBattleFieldStateChanged?.Invoke(state);
    }

    public void Reset()
    {
        mem.Clear();
        var first = new BattleFieldMemory();
        mem.Push(first);
        onBattleFieldStateChanged?.Invoke(first.fieldState);
    }

    public HashSet<GameObject> CurrentTargets()
    {
        return mem.Peek().selectedTargets;
    }


    public GameObject CurrentFirstPerformer()
    {
        return mem.Peek().selectedFirstPerformer;
    }

    public AbstractBattleActionSO CurrentAction()
    {
        return mem.Peek().selectedAction;
    }

    public BattleFieldState CurrentFieldState()
    {
        return mem.Peek().fieldState;
    }


    public string CurrentInfo()
    {
        return mem.Peek().ToString() + " : " +  mem.Count.ToString();
    }

    public string FullInfo()
    {
        return string.Join("\n---\n", mem.Reverse().Select(m => m.ToString()));
    }

    public void UndoLastIfNotEmpty()
    {
        if (mem.Count > 1)
        {
            mem.Pop();
            Debug.Log("UndoLastIfNotEmpty:Undo performed");
        }
    }
    

}

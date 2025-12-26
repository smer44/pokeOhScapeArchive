using UnityEngine;

public enum BattleFieldState
{
    NotSelecting,
    SelectingFirstPerformer,
    SelectingAction,
    SelectingOtherPerformers,
    SelectingTargets,

    AwaitSelectionFinish,
    BattleOver,
    WaitingAnimationEnd,

    AITurn,
}


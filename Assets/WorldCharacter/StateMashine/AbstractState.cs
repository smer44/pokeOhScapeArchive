using UnityEngine;

public abstract class AbstractState
{

    public abstract string GetAnimationName();
    public abstract void EnterState(StateManager ctx);

    public abstract void UpdateState(StateManager ctx);

    public abstract void OnCollisionEnter(StateManager ctx);

    public abstract void FixedUpdateState(StateManager ctx);
}

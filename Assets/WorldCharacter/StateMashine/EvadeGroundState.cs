using UnityEngine;

public class EvadeGroundState : AbstractState
{
    public float moveSpeed { get; private set; } = 20f;
    float evadeTimeMax = 0.7f;
    float evadeTimeCurrent = 0f;


    public static string animationName = "EvadeGround";

    public override string GetAnimationName()
    {
        return animationName;
    }


    public override void EnterState(StateManager ctx)
    {
        evadeTimeCurrent = evadeTimeMax;
        Debug.Log($"Entered {this}");
    }

    public override void UpdateState(StateManager ctx)
    {
        ctx.UpdateToggleRunInput();

        bool stateSwitchIsFree = true;
        if (ctx.isJumpInput)
        {
            ctx.DoJumpImpuls();
            ctx.SwitchState(ctx.runningJumpState);
            //ctx.SetAnimatorBool(ctx.jumpingParam, true);
            stateSwitchIsFree = false;
            return;
        }
        if (evadeTimeCurrent <= 0f)
        {
            ctx.SwitchState(ctx.runningState);
        }


    }

    public override void OnCollisionEnter(StateManager ctx)
    {

    }

    public override void FixedUpdateState(StateManager ctx)
    {
        Vector3 moveDir = ctx.GlobalMoveDirfromInput(ctx.wasdInput);
        Vector3 speed = moveDir * moveSpeed;
        //Debug.Log($"MovingState : FixedUpdateState");
        ctx.SetVelXZRb(speed.x, speed.z);
        //UpdateVisualsForward(ctx, moveDir);
        evadeTimeCurrent -= ctx.fixedDeltaTime;
    }

    
}

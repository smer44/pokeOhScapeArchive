using UnityEngine;

public class IdleState : AbstractState
{
    public float idleMoveClamp = 0.1f;

    private float turningSpeed = 20f;

    public static string animationName = "Idle";

    public override string GetAnimationName()
    {
        return animationName;
    }


    public override void EnterState(StateManager ctx)
    {
        Debug.Log("Entered IdleState");
        ctx.SetVelXZRb(0f, 0f);
    }

    public override void UpdateState(StateManager ctx)
    {
        ctx.UpdateToggleRunInput();

        if (ctx.isJumpInput)
        {
            ctx.DoJumpImpuls();
            ctx.SwitchState(ctx.jumpState);
            //ctx.SetAnimatorBool(ctx.jumpingParam, true);
            return;
        }

        if (ctx.isMovingInput)
        {

            if (ctx.isRunSet)
            {
                ctx.SwitchState(ctx.runningState);
                return;
            }
            else
            {
                ctx.SwitchState(ctx.movingState);
                return;
            }

        }
        if (ctx.IsFallingAfterGround())
        {
            ctx.fallingState.maxSpeed = ctx.movingState.moveSpeed;
            ctx.SwitchState(ctx.fallingState);
            return;
        }

    }

    public override void FixedUpdateState(StateManager ctx)
    {
        // allow for a character to slide a bit from physicks if idle
        ctx.ClampVelXZRb(idleMoveClamp);
        if (!ctx.IsThirdPersonCamera())
            ctx.UpdateVisualsForwardToCameraForward(20f);
    }

    public override void OnCollisionEnter(StateManager ctx)
    {

    }
    

}

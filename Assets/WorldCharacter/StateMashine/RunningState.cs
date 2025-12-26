using UnityEngine;

public class RunningState : AbstractState
{
    public float moveSpeed = 12f;
    private float turningSpeed = 30f;

    public static string animationName = "Running";

    public override string GetAnimationName()
    {
        return animationName;
    }


    public override void EnterState(StateManager ctx)
    {
        Debug.Log("Entered MovingState");
    }

    public override void UpdateState(StateManager ctx)
    {
        ctx.UpdateToggleRunInput();

        if (ctx.IsEvadeInput())
        {
            ctx.SwitchState(ctx.evadeGroundState);
            return;
        }

        if (ctx.isJumpInput)
        {
            ctx.DoJumpImpuls();
            ctx.SwitchState(ctx.runningJumpState);
            //ctx.SetAnimatorBool(ctx.jumpingParam, true);

            return;
        }

        if (!ctx.isMovingInput)
        {

            ctx.SwitchState(ctx.idleState);
            //ctx.SetAnimatorBool(ctx.movingParam, false);
            return;


        }
        else
        {
            if (!ctx.isRunSet)
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

    public override void OnCollisionEnter(StateManager ctx)
    {

    }

    public override void FixedUpdateState(StateManager ctx)
    {
        Vector3 moveDir = ctx.GlobalMoveDirfromInput(ctx.wasdInput);
        Vector3 speed = moveDir * moveSpeed;
        //Debug.Log($"MovingState : FixedUpdateState");
        ctx.SetVelXZRb(speed.x, speed.z);
        
        // if third person, rotate visuals by moving direction
        // else rotate visuals by camera forward direction.
        if (ctx.IsThirdPersonCamera())
        {
            //UpdateVisualsForward(ctx, moveDir);
            ctx.UpdateVisualsForwardByMoveDirection(moveDir, turningSpeed);
        }
        else
        {
            ctx.UpdateVisualsForwardToCameraForward(turningSpeed);
        }
    }


    
}

using UnityEngine;

public class JumpState : AbstractState
{
    public  float midAirAcceleration = 20f;

    public static string animationName = "JumpUp";

    private float noLandTimeMax = 0.1f;
    private float noLandTime = 0.0f;

    public override string GetAnimationName()
    {
        return animationName;
    }


    public override void EnterState(StateManager ctx)
    {
        noLandTime = noLandTimeMax;
        Debug.Log("Entered JumpState");
    }

    public override void UpdateState(StateManager ctx)
    {
        if (ctx.IsEvadeInput())
        {
            ctx.SwitchState(ctx.evadeAirState);
            return;
        }

        if (ctx.IsFallingAfterJump())
        {
            //ctx.SetAnimatorBool(ctx.fallingParam, true);
            //ctx.SetAnimatorBool(ctx.jumpingParam, false);
            ctx.fallingState.maxSpeed = ctx.movingState.moveSpeed;
            ctx.SwitchState(ctx.fallingState);
        }
        if (noLandTime <= 0 && ctx.groundColliderMB.isGrounded)
        {
            //ctx.SetAnimatorBool(ctx.fallingParam, false);
            if (ctx.isMovingInput)
            {

                ctx.SwitchState(ctx.movingState);

                
            }
            else
            {
                ctx.SwitchState(ctx.idleState);              
            }
        }
    }

    public override void OnCollisionEnter(StateManager ctx)
    {

    }

    public override void FixedUpdateState(StateManager ctx)
    {
        noLandTime -= ctx.fixedDeltaTime;
        Vector3 moveDir = ctx.GlobalMoveDirfromInput(ctx.wasdInput);
        Vector3 acceleration = moveDir * (midAirAcceleration * ctx.fixedDeltaTime);
        ctx.AddVelXZRb(acceleration.x, acceleration.z);
        float clampAir = ctx.movingState.moveSpeed ;
        ctx.ClampVelXZRb(clampAir);
    }
}

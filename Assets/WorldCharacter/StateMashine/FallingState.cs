using UnityEngine;

public class FallingState : AbstractState
{
    public  float midAirAcceleration = 0.1f; 
    public  float maxSpeed = 0.0f; 

    public static string animationName = "Falling";

    public override string GetAnimationName()
    {
        return animationName;
    }

    public override void EnterState(StateManager ctx)
    {
        Debug.Log("Entered FallingState");
    }

    public override void UpdateState(StateManager ctx)
    {
        if (ctx.IsEvadeInput())
        {
            ctx.SwitchState(ctx.evadeAirState);
            return;
        }
        
        if (ctx.groundColliderMB.isGrounded)
        {
            //ctx.SetAnimatorBool(ctx.fallingParam, false);
            if (ctx.isMovingInput)
            {
                ctx.SwitchState(ctx.movingState);
                return;
            }
            else
            {
                ctx.SwitchState(ctx.idleState);
                return;
            }
        }
    }

    public override void OnCollisionEnter(StateManager ctx)
    {

    }

    public override void FixedUpdateState(StateManager ctx)
    {
        Vector3 moveDir = ctx.GlobalMoveDirfromInput(ctx.wasdInput);
        Vector3 acceleration = moveDir * midAirAcceleration * ctx.fixedDeltaTime;
        ctx.AddVelXZRb(acceleration.x, acceleration.z);
        ctx.ClampVelXZRb(maxSpeed);
    }
         
}

using UnityEngine;

public class EvadeAirState : AbstractState
{
    public float moveSpeed { get; private set; } = 20f;
    float evadeTimeMax = 0.7f;
    float evadeTimeCurrent = 0f;


    public static string animationName = "EvadeAir";

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


        if (evadeTimeCurrent <= 0f)
        {
            if (ctx.groundColliderMB.isGrounded)
            {
                if (ctx.isMovingInput)
                {
                    ctx.SwitchState(ctx.runningState);

                }
                else
                {
                    ctx.SwitchState(ctx.idleState);
                }
            }
            else
            {
                ctx.fallingState.maxSpeed = moveSpeed; 
                ctx.SwitchState(ctx.fallingState);
            }
            
        }


    }

    public override void OnCollisionEnter(StateManager ctx)
    {

    }

    public override void FixedUpdateState(StateManager ctx)
    {
        evadeTimeCurrent -= ctx.fixedDeltaTime;
        Vector3 moveDir = ctx.GlobalMoveDirfromInput(ctx.wasdInput);
        Vector3 speed = moveDir * moveSpeed;
        //Debug.Log($"MovingState : FixedUpdateState");
        ctx.SetVelXZRb(speed.x, speed.z);
        //UpdateVisualsForward(ctx, moveDir);
        
    }

    
}

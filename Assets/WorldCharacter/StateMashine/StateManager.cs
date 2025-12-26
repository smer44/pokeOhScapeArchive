using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// animator with Crossfade 
public class StateManager : MonoBehaviour
{

    [Header("Animation")]
    [SerializeField] private Animator animator;
    //[SerializeField] public string movingParam = "moving";
    //[SerializeField] public string jumpingParam = "jumping";
    //[SerializeField] public string fallingParam = "falling";
    //[SerializeField] public string cliffHangingParam = "falling";

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 6f;



    public float fixedDeltaTime;
    AbstractState currentState;

    public AbstractState prevState { get; private set; }

    [Header("Input")]
    public InputAction inputAction;



    private Rigidbody rb;


    public IdleState idleState = new IdleState();
    public JumpState jumpState = new JumpState();

    public FallingState fallingState = new FallingState();
    public MovingState movingState = new MovingState();

    public RunningState runningState = new RunningState();

    public RunningJumpState runningJumpState = new RunningJumpState();

    public EvadeGroundState evadeGroundState = new EvadeGroundState();

    public EvadeAirState evadeAirState = new EvadeAirState();

    public Vector2 wasdInput { get; private set; }
    public bool isJumpInput { get; private set; }

    //public bool isEvadeInput { get; private set; }
    public bool isMovingInput { get; private set; }
    public bool isLedgeGrabInput { get; private set; }

    public bool isRunSet { get; private set; } = false;


    [Header("References:")]
    [SerializeField] private Transform cameraTransform; // Assign your child Camera transform here

    [SerializeField] public GroundColliderMB groundColliderMB;

    [SerializeField] public TextMeshProUGUI stateText;

    [SerializeField] public Transform characterVisualsForRotate; // Assign your child Camera transform here

    public Transform forwardTransform { get; private set; }

    private void AwakeRigidbody()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

    }

    private void Awake()
    {
        AwakeRigidbody();
    }

    void Start()
    {
        forwardTransform = cameraTransform;
        SwitchState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        currentState.FixedUpdateState(this);
    }

    void UpdateInputs()
    {
        //UpdateToggleRunInput();
        UpdateWASDInput();
        UpdateJumpInput();
    }

    public void UpdateToggleRunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isRunSet = !isRunSet;
        }
    }

    void UpdateWASDInput()
    {
        wasdInput = inputAction.ReadValue<Vector2>();
        isMovingInput = wasdInput.sqrMagnitude > 1e-6f;
        //Debug.Log($"isMovingInput : {isMovingInput}");
    }
    void UpdateJumpInput()
    {
        isJumpInput = Input.GetKeyDown("space");
    }

    public bool IsEvadeInput()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public void SwitchState(AbstractState state)
    {
        prevState = currentState;
        currentState = state;
        currentState.EnterState(this);
        stateText.text = currentState.ToString();
        animator.CrossFade(state.GetAnimationName(), 0.01f);

    }

    public void DoJumpImpuls()
    {
        var v = rb.linearVelocity;
        v.y = jumpSpeed;
        rb.linearVelocity = v;
        //animator.SetBool(jumpingParam, true);
        //animator.SetBool(movingParam, true);
    }

    public bool IsFallingAfterJump()
    {
        return rb.linearVelocity.y < 0.01f;
    }

    public bool IsFallingAfterGround()
    {
        return rb.linearVelocity.y < -0.3f;
    }



    //public void SetAnimatorBool(string param, bool value)
    //{
    //    animator.SetBool(param, value);
    //}


    public Vector3 GlobalMoveDirfromInput(Vector2 input)
    {
        Vector3 forward = Vector3.ProjectOnPlane(forwardTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(forwardTransform.right, Vector3.up).normalized;
        Vector3 moveDir = forward * input.y + right * input.x;
        moveDir.Normalize();
        //Debug.Log($"GlobalMoveDirfromInput : moveDir: {moveDir}");
        return moveDir;
    }

    public void SetVelXZRb(float vx, float vz)
    {
        Vector3 v = rb.linearVelocity;
        v.x = vx;
        v.z = vz;
        rb.linearVelocity = v;
    }

    public void AddVelXZRb(float vx, float vz)
    {
        Vector3 v = rb.linearVelocity;
        v.x += vx;
        v.z += vz;
        rb.linearVelocity = v;
    }

    //clamp should be applied to Vector2
    public void ClampVelXZRb(float vmax)
    {
        Vector3 vel = rb.linearVelocity;
        Vector3 velXZ = new Vector3(vel.x, 0f, vel.z);
        float mag = velXZ.magnitude;
        if (mag > vmax)
        {
            velXZ = velXZ.normalized * vmax;
            vel = new Vector3(velXZ.x, vel.y, velXZ.z);
            rb.linearVelocity = vel;
        }

    }

    void OnEnable()
    {
        inputAction.Enable();
    }


    void OnDisable()
    {
        inputAction.Disable();
    }

    public void InputActionOnOff(bool on)
    {
        if (on)
        {
            inputAction.Enable();
        }
        else
        {
            inputAction.Disable();
        }
    }

    public bool IsThirdPersonCamera()
    {
        CameraRotator caro = this.gameObject.GetComponent<CameraRotator>();
        return caro.IsThirdPerson();
    }

    public void UpdateVisualsForwardToCameraForward(float lerpSpeed)
    {
        float delta = this.fixedDeltaTime;
        Transform characterVisuals = characterVisualsForRotate;

        Vector3 newForward = cameraTransform.forward;
        newForward.y = 0f;

        characterVisuals.forward = Vector3.Lerp(characterVisuals.forward, newForward.normalized, lerpSpeed * delta);

    }

    public void  UpdateVisualsForwardForFirstPerson()       
    {
        if (!this.IsThirdPersonCamera())
            UpdateVisualsForwardToCameraForward(20f);
           
    }
    public void UpdateVisualsForwardByMoveDirection(Vector3 moveDir, float turningSpeed)
    {
        float delta = fixedDeltaTime;
        Transform characterVisuals = characterVisualsForRotate;
        characterVisuals.forward = Vector3.Lerp(characterVisuals.forward, moveDir, turningSpeed * delta);

    }

}

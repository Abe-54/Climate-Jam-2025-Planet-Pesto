
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPP : MonoBehaviour
{
    [Header("Scanner Settings")]
    //A variable for the scammer game object itself
    [SerializeField] private GameObject scannerObj;
    
    [Header("Movement Settings")]
    public bool canMove = true;
    public float moveSpeed = 10f;
    public float runAccelAmount = 90f;
    public float runDeccelAmount = 60f;
    public float accelInAir = 0.65f;
    public float deccelInAir = 0.65f;
    public bool IsFacingRight = true;

    [Header("Jump Settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded;
    public float jumpForce = 16f;
    public float jumpCutGravityMult = 2f;
    public float fallGravityMult = 1.5f;
    public float coyoteTime = 0.1f;
    public float jumpInputBufferTime = 0.1f;

    [Header("Wall Jump Settings")]
    public Transform frontWallCheck;
    public Transform backWallCheck;
    public Vector2 wallCheckSize = new Vector2(0.5f, 1f);
    public Vector2 wallJumpForce = new Vector2(8f, 16f);
    public float wallJumpTime = 0.2f;
    public float wallJumpRunLerp = 0.5f;
    public float slideSpeed = -5f;
    public float slideAccel = 5f;

    [Header("Dash Settings")]
    public int dashAmount = 1;
    public float dashSpeed = 20f;
    public float dashAttackTime = 0.15f;
    public float dashEndTime = 0.1f;
    public Vector2 dashEndSpeed = new Vector2(15f, 15f);
    public float dashInputBufferTime = 0.1f;
    public float dashRefillTime = 0.1f;
    public float dashSleepTime = 0.05f;
    public float dashEndRunLerp = 0.6f;

    // Components
    private Rigidbody2D rb2d;
    private float defaultGravityScale;

    // Input
    private Vector2 moveInput;
    private bool jumpInput;
    private bool dashInput;

    // State
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }

    // Timers
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    //A boolean keeping track of the current npc that is interactable
    private NPCPP curNPC;

    // Jump variables
    private bool _isJumpCut;
    private bool _isJumpFalling;

    // Wall Jump variables
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    // Dash variables
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;
    
    //Scanning Variables
    //A boolean keeping track whether or not the scanner is active 
    private bool scanOn = false;


    EventBindingPP<ScannerOnEvent> scannerOnEvent;

    private void OnEnable()
    {
        scannerOnEvent = new EventBindingPP<ScannerOnEvent>(HandleScannerOnEvent);
        EventBusPP<ScannerOnEvent>.Register(scannerOnEvent);
    }

    private void OnDisable()
    {
        EventBusPP<ScannerOnEvent>.Deregister(scannerOnEvent);
    }
    void HandleScannerOnEvent(ScannerOnEvent scannerOnEvent)
    {
       
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb2d = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb2d.gravityScale;
        _dashesLeft = dashAmount;
    }

    void Update()
    {
        if (!canMove) return;

        UpdateTimers();
        CheckCollisions();
        HandleJumpLogic();
        HandleDashLogic();
        HandleSlideLogic();
        HandleGravity();
        HandleFacing();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        if (!IsDashing)
        {
            Run(IsWallJumping ? wallJumpRunLerp : 1f);
        }
        else if (_isDashAttacking)
        {
            Run(dashEndRunLerp);
        }

        if (IsSliding)
            Slide();
    }

    void UpdateTimers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
    }

    void CheckCollisions()
    {
        if (!IsDashing && !IsJumping)
        {
            // Ground Check
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (isGrounded)
                LastOnGroundTime = coyoteTime;

            // Wall Checks
            bool frontWall = Physics2D.OverlapBox(frontWallCheck.position, wallCheckSize, 0, groundLayer);
            bool backWall = Physics2D.OverlapBox(backWallCheck.position, wallCheckSize, 0, groundLayer);

            if ((frontWall && IsFacingRight) || (backWall && !IsFacingRight))
                LastOnWallRightTime = coyoteTime;

            if ((frontWall && !IsFacingRight) || (backWall && IsFacingRight))
                LastOnWallLeftTime = coyoteTime;

            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
    }

    void HandleJumpLogic()
    {
        if (IsJumping && rb2d.linearVelocity.y < 0)
        {
            IsJumping = false;
            _isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;
            _isJumpFalling = false;
        }

        if (!IsDashing)
        {
            // Regular Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }
            // Wall Jump
            else if (CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
                WallJump(_lastWallJumpDir);
            }
        }
    }

    void HandleDashLogic()
    {
        if (CanDash() && LastPressedDashTime > 0)
        {
            StartCoroutine(Sleep(dashSleepTime));

            if (moveInput != Vector2.zero)
                _lastDashDir = moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;

            StartCoroutine(StartDash(_lastDashDir));
        }
    }

    void HandleSlideLogic()
    {
        if (CanSlide() && ((LastOnWallLeftTime > 0 && moveInput.x < 0) ||
                          (LastOnWallRightTime > 0 && moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
    }

    void HandleGravity()
    {
        if (!_isDashAttacking)
        {
            if (IsSliding)
            {
                rb2d.gravityScale = 0;
            }
            else if (_isJumpCut)
            {
                rb2d.gravityScale = defaultGravityScale * jumpCutGravityMult;
            }
            else if (rb2d.linearVelocity.y < 0)
            {
                rb2d.gravityScale = defaultGravityScale * fallGravityMult;
            }
            else
            {
                rb2d.gravityScale = defaultGravityScale;
            }
        }
        else
        {
            rb2d.gravityScale = 0;
        }
    }

    void HandleFacing()
    {
        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);
    }

    void Run(float lerpAmount)
    {
        float targetSpeed = moveInput.x * moveSpeed;
        targetSpeed = Mathf.Lerp(rb2d.linearVelocity.x, targetSpeed, lerpAmount);

        float accelRate = (LastOnGroundTime > 0) ?
            ((Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount) :
            ((Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir);

        float speedDif = targetSpeed - rb2d.linearVelocity.x;
        float movement = speedDif * accelRate;

        rb2d.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float force = jumpForce;
        if (rb2d.linearVelocity.y < 0)
            force -= rb2d.linearVelocity.y;

        rb2d.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void WallJump(int dir)
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        // Force flip away from wall
        bool shouldFaceRight = dir > 0;
        if (IsFacingRight != shouldFaceRight)
            FlipX();

        Vector2 force = new Vector2(wallJumpForce.x, wallJumpForce.y);
        force.x *= dir;

        if (Mathf.Sign(rb2d.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= rb2d.linearVelocity.x;

        if (rb2d.linearVelocity.y < 0)
            force.y -= rb2d.linearVelocity.y;

        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    void Slide()
    {
        if (rb2d.linearVelocity.y > 0)
        {
            rb2d.AddForce(-rb2d.linearVelocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        float speedDif = slideSpeed - rb2d.linearVelocity.y;
        float movement = speedDif * slideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime),
                              Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        rb2d.AddForce(movement * Vector2.up);
    }

    IEnumerator StartDash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;
        float startTime = Time.time;
        _dashesLeft--;
        _isDashAttacking = true;
        rb2d.gravityScale = 0;

        while (Time.time - startTime <= dashAttackTime)
        {
            rb2d.linearVelocity = dir.normalized * dashSpeed;
            yield return null;
        }

        startTime = Time.time;
        _isDashAttacking = false;
        rb2d.gravityScale = defaultGravityScale;
        rb2d.linearVelocity = dashEndSpeed.magnitude * dir.normalized;

        while (Time.time - startTime <= dashEndTime)
        {
            yield return null;
        }

        IsDashing = false;
    }

    IEnumerator RefillDash(int amount)
    {
        _dashRefilling = true;
        yield return new WaitForSeconds(dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(dashAmount, _dashesLeft + amount);
    }

    IEnumerator Sleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    // Input Methods
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            LastPressedJumpTime = jumpInputBufferTime;
        }

        if (ctx.canceled && (CanJumpCut() || CanWallJumpCut()))
        {
            _isJumpCut = true;
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            LastPressedDashTime = dashInputBufferTime;
        }
    }
    
    public void OnScanner(InputAction.CallbackContext ctx)
    {
        if (!scanOn && ctx.performed)
        {
            EventBusPP<ScannerOnEvent>.Raise(new ScannerOnEvent { });
            scannerObj.SetActive(true);
            scanOn = true;
            scannerObj.transform.SetLocalPositionAndRotation(new Vector3(0, 4, 0), new Quaternion(0, 0, 0, 0));
            
        }
        else if(scanOn && ctx.performed)
        {
            
            scannerObj.SetActive(false);
            scanOn = false;
            scannerObj.transform.SetLocalPositionAndRotation(new Vector3(0, 4, 0),new Quaternion(0,0,0,0));
        }
    }

    // Check Methods
    bool CanJump() => LastOnGroundTime > 0 && !IsJumping;

    bool CanWallJump() => LastPressedJumpTime > 0 && LastOnWallTime > 0 &&
                         LastOnGroundTime <= 0 && (!IsWallJumping ||
                         (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) ||
                         (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));

    bool CanJumpCut() => IsJumping && rb2d.linearVelocity.y > 0;

    bool CanWallJumpCut() => IsWallJumping && rb2d.linearVelocity.y > 0;

    bool CanDash()
    {
        if (!IsDashing && _dashesLeft < dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(RefillDash(1));
        }
        return _dashesLeft > 0;
    }

    bool CanSlide() => LastOnWallTime > 0 && !IsJumping && !IsWallJumping &&
                      !IsDashing && LastOnGroundTime <= 0;

    void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            FlipX();
    }

    void FlipX()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!scanOn && ctx.performed && curNPC)
        {
            curNPC.TriggerInteract();
        }
    }


    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (frontWallCheck != null && backWallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(frontWallCheck.position, wallCheckSize);
            Gizmos.DrawWireCube(backWallCheck.position, wallCheckSize);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            curNPC = collision.gameObject.GetComponent<NPCPP>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "NPC")
            {
                curNPC = null;
            }
        }

    }

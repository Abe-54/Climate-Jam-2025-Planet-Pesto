
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPP : MonoBehaviour
{
    [Header("Scanner Settings")]
    //A variable for the scammer game object itself
    [SerializeField] private GameObject scannerObj;
    
    [Header("Movement Settings")]
    public bool canPlayerMove = true;
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
    [SerializeField] private bool infiniteDash;
    public int dashSteamUsageAmt = 10;
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
    private SteamControllerPP steamController;
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
    //A value keeping track of the current npc that is interactable
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

    //Events
    EventBindingPP<ScannerOnEvent> scannerOnEvent;
    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;

    private void OnEnable()
    {
        conversationEndEvent = new EventBindingPP<ConversationEndEvent>(HandleConversationEndEvent);
        EventBusPP<ConversationEndEvent>.Register(conversationEndEvent);

        scannerOnEvent = new EventBindingPP<ScannerOnEvent>(HandleScannerOnEvent);
        EventBusPP<ScannerOnEvent>.Register(scannerOnEvent);

        conversationStartEvent = new EventBindingPP<ConversationStartEvent>(HandleConversationStartEvent);
        EventBusPP<ConversationStartEvent>.Register(conversationStartEvent);

    }

    private void OnDisable()
    {
        EventBusPP<ScannerOnEvent>.Deregister(scannerOnEvent);
        EventBusPP<ConversationEndEvent>.Deregister(conversationEndEvent);
        EventBusPP<ConversationStartEvent>.Deregister(conversationStartEvent);
    }
    void HandleScannerOnEvent(ScannerOnEvent scannerOnEvent)
    {
       
    }
    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
        SetCanMove(false);
     
    }

    public void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        SetCanMove(true);
    }

    void Start()
    {
    
        rb2d = GetComponent<Rigidbody2D>();
        steamController = GetComponent<SteamControllerPP>();
        defaultGravityScale = rb2d.gravityScale;
        _dashesLeft = dashAmount;
    }

    void Update()
    {
     
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

    // Update Timers for Player Movement Options
    // Void Function -> subtract the delta time from the Last X Time
    void UpdateTimers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
    }

    // Physics Collision
    // Checks if player is on Ground/Wall
    // Also where Coyote Time is Reset
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

    // Normal Jump Logic is handled here
    // Also sets jump variables, such as IsJumping/IsWallJumping
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

    // This is where Dash Logic is handled
    // It runs the dashSleep Coroutine and resets related variables
    // This is also where Steam will get removed
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

            // REMOVING STEAM HERE
            if (!infiniteDash)
            {
                steamController.RemoveSteam(dashSteamUsageAmt);
            }

            FindAnyObjectByType<PlayerAudioControllerPP>().PlayMovementSFX(PlayerSFXType.dashing);
            // STARTING THE DASH HERE
            StartCoroutine(StartDash(_lastDashDir));
        }
    }

    // This is where the wall sliding logic to detect if the
    // player is pushing onto the wall
    void HandleSlideLogic()
    {
        if (CanSlide() && ((LastOnWallLeftTime > 0 && moveInput.x < 0) ||
                          (LastOnWallRightTime > 0 && moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
    }

    // This controller uses different gravities based on different states
    // This is where the player's rigidbody2d gravity gets modified
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
    
    // Simple function to alter where the player's facing
    void HandleFacing()
    {
        if (moveInput.x != 0 && (moveInput.x > 0) != IsFacingRight)
            FlipX();
    }

    // This is how the player runs in our game
    void Run(float lerpAmount)
    {
        //We find our final run speed
        float targetSpeed = moveInput.x * moveSpeed;
        targetSpeed = Mathf.Lerp(rb2d.linearVelocity.x, targetSpeed, lerpAmount);

        // Find the acceleration rate
        float accelRate = (LastOnGroundTime > 0) ?
            ((Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount) :
            ((Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir);
        float speedDif = targetSpeed - rb2d.linearVelocity.x;
        float movement = speedDif * accelRate;
    
        // Finally add the movement to the player via rigidbody2d force
        rb2d.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    // This is where the player jumps in our game
    // and resets any variables related to it
    void Jump()
    {
        FindAnyObjectByType<PlayerAudioControllerPP>().PlayMovementSFX(PlayerSFXType.jumping);
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
    
        // Find the amount of force to give
        float force = jumpForce;
        // This dampens the player's falling velocity
        if (rb2d.linearVelocity.y < 0)
            force -= rb2d.linearVelocity.y;
    
        // Finally adding the jump force to the player via rigidbody2d impulse
        rb2d.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    // This is where the player wall jumps in our game
    // and resets any variables related to it
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

        // Figure out how much wall jump force to give the player
        Vector2 force = new Vector2(wallJumpForce.x, wallJumpForce.y);
        force.x *= dir;

        // Counteracts the player's current horizontal velocity
        if (Mathf.Sign(rb2d.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= rb2d.linearVelocity.x;
        
        // Counteracts the player's current vertical velocity to ensure it jumps upward 
        if (rb2d.linearVelocity.y < 0)
            force.y -= rb2d.linearVelocity.y;

        // Finally adding the wall jump force to the player via rigidbody2d impulse
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    // This is where the ACTUAL wall sliding happens
    void Slide()
    {
        // checks if the player is moving up
        if (rb2d.linearVelocity.y > 0)
        {
            // Immediately cancels the upward velocity by applying an equal and opposite impulse force.
            rb2d.AddForce(-rb2d.linearVelocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        // Find the target speed to slide
        float speedDif = slideSpeed - rb2d.linearVelocity.y;
        float movement = speedDif * slideAccel;
        // This prevents overshooting the target velocity by limiting the force to what's needed to reach the target speed in one physics frame
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime),
                              Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        // Finally Add the force to the player to slow down their fall
        rb2d.AddForce(movement * Vector2.up);
    }

    // This is the ACTUAL dash code / coroutine
    // It requires the direction to dash in and resets any variables needed DURING the dash
    IEnumerator StartDash(Vector2 dir)
    {

        // Reseting necessery variables
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;
        float startTime = Time.time;
        if (!infiniteDash)
        {
            _dashesLeft--;
        }
        _isDashAttacking = true;
        rb2d.gravityScale = 0;
    
        // THE DASH
        while (Time.time - startTime <= dashAttackTime)
        {
            // continuously give the player speed in a specified direction
            rb2d.linearVelocity = dir.normalized * dashSpeed;
            yield return null;
        }
        
        // Stops the dash & resets all related variables
        startTime = Time.time;
        _isDashAttacking = false;
        rb2d.gravityScale = defaultGravityScale;
        rb2d.linearVelocity = dashEndSpeed.magnitude * dir.normalized;

        while (Time.time - startTime <= dashEndTime)
        {
            yield return null;
        }
    
        // Disable Dash State
        IsDashing = false;
    }
    
    // Co-Routine to add X number of dashes to player
    IEnumerator RefillDash(int amount)
    {
        _dashRefilling = true;
        yield return new WaitForSeconds(dashRefillTime);
        _dashRefilling = false;
        // This caps the number of dashes left to the amount the player started with
        _dashesLeft = Mathf.Min(dashAmount, _dashesLeft + amount);
    }
    
    // This is a Sleep helper function to pause the player for X amount of seconds
    IEnumerator Sleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    // Input Methods
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (canPlayerMove)
        {

            moveInput = ctx.ReadValue<Vector2>();
        }
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
        if (!IsDashing && _dashesLeft < dashAmount && LastOnGroundTime > 0 && !_dashRefilling && steamController.HasSteam())
        {
            StartCoroutine(RefillDash(1));
        }
        return _dashesLeft > 0;
    }

    bool CanSlide() => LastOnWallTime > 0 && !IsJumping && !IsWallJumping &&
                      !IsDashing && LastOnGroundTime <= 0;
    

    // Simple Function to Flip the players X facing direction
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
            
            rb2d.linearVelocityX = 0;
            curNPC.Interact();
        }
    }

    public void SetCanMove(bool canMove)
    {
        canPlayerMove = canMove;
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

    public void SetInfiniteDash(bool newBool)
    {
        infiniteDash = newBool;
    }

    }

     

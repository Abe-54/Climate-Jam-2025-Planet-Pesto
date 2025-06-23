using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPP : MonoBehaviour
{
    /*
    TODO: 
        [] Get Player Dashing
        [] Get Player Wall-Jumping
    */
    
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    
    [Header("Jump Settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded;
    public float jumpForce = 10f;
   
    private Rigidbody2D rb2d;
    private Vector2 moveInput;
    private bool jumpInput;

    //A variable for the scammer game object itself
    [SerializeField] private GameObject scannerObj;

    //A boolean keeping track whether or not the scanner is active 
    private bool scanOn = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        rb2d.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.linearVelocityY);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, jumpForce);
        }

        if (ctx.canceled && rb2d.linearVelocityY > 0f)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, rb2d.linearVelocityY * 0.5f);
        }
    }

    public void OnScanner(InputAction.CallbackContext ctx)
    {
        if (!scanOn)
        {
            scannerObj.SetActive(true);
            scanOn = true;
        }
        else
        {
            
            scannerObj.SetActive(false);
            scanOn = false;
            scannerObj.transform.SetLocalPositionAndRotation(new Vector3(0, 1, 0),new Quaternion(0,0,0,0));
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

   
}

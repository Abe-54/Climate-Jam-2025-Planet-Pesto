using UnityEngine;
using UnityEngine.InputSystem;

public class ScannerControllerPP : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    private Rigidbody2D rb2d;
    private Vector2 moveInput;
    private NPCPP curScanObj;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    //Method for controlling movement
    public void OnLook(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnScanInitiate(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && curScanObj) 
        {
            curScanObj.Scan();
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        rb2d.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Scanner COLLSION");
        if(collision.gameObject.tag == "NPC")
        {
            curScanObj = collision.gameObject.GetComponent<NPCPP>();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            curScanObj = null;
        }

    }
}

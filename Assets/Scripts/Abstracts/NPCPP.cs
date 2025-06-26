using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//An abstract class which defines the abilities of NPC's within the game
public abstract class NPCPP : MonoBehaviour, IIInteractablePP, IScanablePP
{
    //Sprite to indicate that something is interactable
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private SpriteRenderer interactIcon;
    [SerializeField] private DialogueTextPP dialogueText;
    [SerializeField] private DialogueTextPP scannerText;
    [SerializeField] private GameObject scannerLayer;
    [SerializeField] private DialogueControllerPP dialogueController;
    //Variable to keep track of a players location
    public Transform playerTrans { get; private set; }
    private bool isBeingScanned = false;

    private const float INTERACT_RANGE = 5f;

    //Courotine for flashing  
    private Coroutine scanFlashEvent;

    private const float FLASH_TIME = .5f;

    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ScannerOnEvent> scannerOnEvent;

    private void OnEnable()
    {
        conversationEndEvent = new EventBindingPP<ConversationEndEvent>(HandleConversationEndEvent);
        EventBusPP<ConversationEndEvent>.Register(conversationEndEvent);

        scannerOnEvent = new EventBindingPP<ScannerOnEvent>(HandleScannerOnEvent);
        EventBusPP<ScannerOnEvent>.Register(scannerOnEvent);
    }

    private void OnDisable()
    {
        EventBusPP<ConversationEndEvent>.Deregister(conversationEndEvent);
        EventBusPP<ScannerOnEvent>.Deregister(scannerOnEvent);
    }



    public abstract void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent);

    public void HandleScannerOnEvent(ScannerOnEvent scannerOnEvent)
    {
        Debug.Log("EHEHRHENRER");
        StartCoroutine(ScanFlash());
    }

    private void Start()
    {
        //Grab player location
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //Abstract method for interacting with NPC
    public abstract void Interact();

    //Abstract method for scanning NPC
    public abstract void Scan();

    public void TriggerInteract()
    {
        Interact();
    }

    //Method to toggle the interact sprite
    public void InteractSpriteToggle()
    {

        //Check if it is already on 
        if (interactSprite.gameObject.activeSelf)
        {
            interactSprite.gameObject.SetActive(false);
        }

        else if (!interactSprite.gameObject.activeSelf)
        {
            //Check which control scheme we are using to effect InputIcon
            
            interactSprite.gameObject.SetActive(true);
        }

    }

    //Method to keep track of whether or not the player is within range to interact
    public bool IsWithinRange()
    {
        if (Vector2.Distance(transform.position, playerTrans.position) < INTERACT_RANGE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scanner")
        {
            Debug.Log("SCANNER ENTERED");
            isBeingScanned = true;
            scannerLayer.SetActive(true);
            InteractSpriteToggle();

        }
        if (collision.gameObject.tag == "Player")
        {
            InteractSpriteToggle();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scanner")
        {
            Debug.Log("SCANNER SCANNER LEFT");
            isBeingScanned = false;
            scannerLayer.SetActive(false);
            InteractSpriteToggle();
        }
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hey");
            InteractSpriteToggle();
        }

    }

    private IEnumerator ScanFlash()
    {
        scannerLayer.SetActive(true);

        yield return new WaitForSeconds(FLASH_TIME);

        scannerLayer.SetActive(false);

    }

    public DialogueTextPP GetDialogueText()
    {
        return dialogueText;
    }

    public void  SetDialogueText(DialogueTextPP newDialogue)
    {
        dialogueText = newDialogue;
    }

    public DialogueTextPP GetScannerText()
    {
        return scannerText;
    }

    public DialogueControllerPP GetDialogueController()
    {
        return dialogueController;
    }

    public bool GetIsBeingScanned()
    {
        return isBeingScanned;
    }
    public SpriteRenderer GetInteractSprite()
    {
        return interactSprite;
    }

    public NPCPP GetNPC()
    {
        return this;
    }

 
}

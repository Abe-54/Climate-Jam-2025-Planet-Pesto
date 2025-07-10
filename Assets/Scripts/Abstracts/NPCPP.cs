using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//An abstract class which defines the abilities of NPC's within the game
public abstract class NPCPP : MonoBehaviour, IIInteractablePP, IScanablePP
{
    //Sprite to indicate that something is interactable
    [SerializeField] private DialogueTextPP dialogueText;
    [SerializeField] private DialogueTextPP scannerText;
    [SerializeField] private DialogueControllerPP dialogueController;
    [SerializeField] private Animator animator;
    [SerializeField] private bool interactSpriteAvailable = true;
    private GameObject interact;
    
    //Variable to keep track of a players location
    private bool isBeingScanned = false;

    //Variables controlling whether or not this object can be scanned or interacted with 
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private bool isScannable = true;



    private const float FLASH_TIME = .5f;

    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;
    EventBindingPP<ScannerOnEvent> scannerOnEvent;

    private void OnEnable()
    {
        conversationEndEvent = new EventBindingPP<ConversationEndEvent>(HandleConversationEndEvent);
        EventBusPP<ConversationEndEvent>.Register(conversationEndEvent);

        conversationStartEvent = new EventBindingPP<ConversationStartEvent>(HandleConversationStartEvent);
        EventBusPP<ConversationStartEvent>.Register(conversationStartEvent);

        scannerOnEvent = new EventBindingPP<ScannerOnEvent>(HandleScannerOnEvent);
        EventBusPP<ScannerOnEvent>.Register(scannerOnEvent);


    }

    private void OnDisable()
    {
        EventBusPP<ConversationEndEvent>.Deregister(conversationEndEvent);
        EventBusPP<ConversationStartEvent>.Deregister(conversationStartEvent);
        EventBusPP<ScannerOnEvent>.Deregister(scannerOnEvent);
    }
    public void HandleScannerOnEvent(ScannerOnEvent scannerOnEvent)
    {
        if (isScannable)
        {
            animator.SetTrigger("ScanFlash");
        }
    }


    public abstract void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent);

    public abstract void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent);

   

    private void Start()
    {
        dialogueController = FindFirstObjectByType<DialogueControllerPP>();
 
        if (!dialogueController)
        {
            dialogueController = Object.FindFirstObjectByType<DialogueControllerPP>();
        }
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        StartUp();
    }

    //Start up method abstractions
    public abstract void StartUp();

    //Abstract method for interacting with NPC
    public abstract void Interact();

    //Abstract method for scanning NPC
    public abstract void Scan();

    //Method to toggle the interact sprite
    public void InteractSpriteToggle(bool state)
    {
        
        if (state)
        {
           
            switch (FindAnyObjectByType<PlayerInput>().currentControlScheme)
            {
                case "Keyboard&Mouse":
                  
                    interact = Instantiate(Resources.Load<GameObject>("Prefab/InteractableSpriteKeyboard"),this.transform, worldPositionStays: false);
                    break;
                case "Gamepad":
                    interact = Instantiate(Resources.Load<GameObject>("Prefab/InteractableSpriteController"), this.transform, worldPositionStays: false);
                    break;
            }

            
        }
        else if(!state && interact)
        {
            Destroy(interact);
            interact = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Scanner" && isScannable)
        {
            Debug.Log("SCANNER ENTERED");
            isBeingScanned = true;
            animator.SetBool("BeingScanned", true);
            if (interactSpriteAvailable)
            {
                InteractSpriteToggle(true);
            }
            

        }
        if (collision.gameObject.tag == "Player" && isInteractable)
        {
            if (interactSpriteAvailable)
            {
                InteractSpriteToggle(true);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scanner" && isScannable)
        {
            Debug.Log("SCANNER SCANNER LEFT");
            isBeingScanned = false;
            animator.SetBool("BeingScanned", false);
            InteractSpriteToggle(false);
            
        }
        if (collision.gameObject.tag == "Player" && isInteractable)
        {
            
            InteractSpriteToggle(false);
        }

    }



    public bool GetIsScannable()
    {
        return isScannable;
    }
    public void SetIsScannable(bool newBool)
    {
        isScannable = newBool;
    }

    public bool GetIsInteractable()
    {
        return isInteractable;
    }

    public void SetIsInteractable(bool newBool)
    {
        isInteractable = newBool;
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

    public NPCPP GetNPC()
    {
        return this;
    }

 
}

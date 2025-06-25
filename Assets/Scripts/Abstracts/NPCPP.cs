using UnityEngine;
using UnityEngine.InputSystem;

//An abstract class which defines the abilities of NPC's within the game
public abstract class NPCPP : MonoBehaviour, IIInteractablePP, IScanablePP
{
    //Sprite to indicate that something is interactable
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private DialogueTextPP dialogueText;
    [SerializeField] private DialogueTextPP scannerText;
    [SerializeField] private GameObject scannerLayer;
    [SerializeField] private DialogueControllerPP dialogueController;
    //Variable to keep track of a players location
    private Transform playerTrans;
    private bool isBeingScanned = false;

    private const float INTERACT_RANGE = 5f;

   

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

    public void InteractSpriteToggle()
    {
        if (interactSprite.gameObject.activeSelf)
        {
            interactSprite.gameObject.SetActive(false);
        }

        else if (!interactSprite.gameObject.activeSelf)
        {
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

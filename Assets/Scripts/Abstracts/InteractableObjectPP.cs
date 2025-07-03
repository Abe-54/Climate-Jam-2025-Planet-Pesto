using System.Collections;
using UnityEngine;

public abstract class InteractableObjectPP : MonoBehaviour, IScanablePP
{
    //Sprite to indicate that something is interactable
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private DialogueTextPP scannerText;
    [SerializeField] private GameObject scannerLayer;
    [SerializeField] private DialogueControllerPP dialogueController;

    private bool isBeingScanned = false;

    //Courotine for flashing  
    private Coroutine scanFlashEvent;

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

    public abstract void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent);

    public abstract void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent);

    public void HandleScannerOnEvent(ScannerOnEvent scannerOnEvent)
    {
        StartCoroutine(ScanFlash());
    }

    //Abstract method for scanning NPC
    public abstract void Scan();

    //Method to toggle the interact sprite
    public void InteractSpriteToggle(bool state)
    {
        interactSprite.gameObject.SetActive(state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scanner")
        {
            Debug.Log("SCANNER ENTERED");
            isBeingScanned = true;
            scannerLayer.SetActive(true);
            InteractSpriteToggle(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scanner")
        {
            Debug.Log("SCANNER SCANNER LEFT");
            isBeingScanned = false;
            scannerLayer.SetActive(false);
            InteractSpriteToggle(false);
        }
    }

    private IEnumerator ScanFlash()
    {
        scannerLayer.SetActive(true);

        yield return new WaitForSeconds(FLASH_TIME);

        scannerLayer.SetActive(false);
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
    
    public InteractableObjectPP GetInteractableObject()
    {
        return this;
    }
}


using UnityEngine;


public class OleanPP : NPCPP, ITalkablePP
{

    EventBindingPP<ConversationEndEvent> conversationEndEvent;


    private void OnEnable()
    {
        conversationEndEvent = new EventBindingPP<ConversationEndEvent>(HandleConversationEndEvent);
        EventBusPP<ConversationEndEvent>.Register(conversationEndEvent);
    }

    private void OnDisable()
    {
        EventBusPP<ConversationEndEvent>.Deregister(conversationEndEvent);
    }

    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        if (conversationEndEvent.eventName == "OL1")
        {
            SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/Lab1 Dialogue/OL1.5"));
        }
    }

    //Code handeling what happens when she is scanned 
    public override void Scan()
    {
        //Ensure it's not null
        if (GetScannerText())
        {
            Talk(GetScannerText());
        }
    }

    public override void Interact()
    {
        if (GetDialogueText())
        {
            Talk(GetDialogueText());
        }
    }
    /*
    //Upon the player pressing the interact key, if they are within range then initiate the interaction
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        print("Interact button pressed");
        if (ctx.performed && IsWithinRange())
        {
            Interact();
        }
    }
    */

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);

    }




}

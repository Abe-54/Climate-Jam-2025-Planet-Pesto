
using UnityEngine;


public class AlenaPP : NPCPP, ITalkablePP
{
    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        if (conversationEndEvent.eventName == "Alina1")
        {
            SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/TestDialogues/TestAlina1"));
        }
        else if (conversationEndEvent.eventName == "AL1")
        {
            SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/Lab1 Dialogue/AL1.5"));
        }
        else if (conversationEndEvent.eventName == "OL1")
        {
            SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/Lab1 Dialogue/AL2"));
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

    public override void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
        InteractSpriteToggle(false);
    }
}


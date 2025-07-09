
using UnityEngine;


public class OleanPP : NPCPP, ITalkablePP
{

    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        if (conversationEndEvent.eventName == "OL1")
        {
            SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/Lab1 Dialogue/OL1.5"));
        }
    }
    public override void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
        InteractSpriteToggle(false);
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

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);

    }

    public override void StartUp()
    {
        
    }
}

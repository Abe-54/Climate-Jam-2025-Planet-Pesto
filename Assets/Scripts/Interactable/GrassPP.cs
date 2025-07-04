using UnityEngine;

public class GrassPP : InteractableObjectPP
{
    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
       
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

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);
    }
}

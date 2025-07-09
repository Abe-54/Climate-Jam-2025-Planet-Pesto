using UnityEngine;

public class TramNPC : NPCPP, ITalkablePP
{
    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
 
    }

    public override void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
      
    }

    public override void Interact()
    {
        if (GetDialogueText())
        {
            Talk(GetDialogueText());
        }
    }

    public override void Scan()
    {
      
    }

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);
    }

 
}

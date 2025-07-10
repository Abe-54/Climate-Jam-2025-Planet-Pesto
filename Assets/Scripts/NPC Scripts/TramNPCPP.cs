using UnityEngine;

public class TramNPC : NPCPP, ITalkablePP
{
    [SerializeField] private DialogueTextPP returnTripDialogue;
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

    public override void StartUp()
    {
        
    }

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);
    }

 
}

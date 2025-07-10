using UnityEngine;

public class TramNPC : NPCPP, ITalkablePP
{
    [SerializeField] private bool rightNPC;
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
        if (GameManagerPP.instance.GetCreatureFlag())
        {
            if (rightNPC)
            {
                SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/Tram2 Dialogue/Tram2Right"));
            }
            else
            {
                SetDialogueText(Resources.Load<DialogueTextPP>("Dialogue/Tram2 Dialogue/Tram2Left"));
            }
        }
    }

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);
    }

 
}

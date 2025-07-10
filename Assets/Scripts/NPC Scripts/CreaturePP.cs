using UnityEngine;

public class CreaturePP : NPCPP
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

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);

    }

    public override void Scan()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetIsScannable(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartUp()
    {
        
    }
}

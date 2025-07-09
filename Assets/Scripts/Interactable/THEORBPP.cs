using UnityEngine;

public class THEORBPP : NPCPP
{
    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
   
    }

    public override void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
        InteractSpriteToggle(false);
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
        if (GetScannerText())
        {
            Talk(GetScannerText());
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartUp()
    {
       
    }
}

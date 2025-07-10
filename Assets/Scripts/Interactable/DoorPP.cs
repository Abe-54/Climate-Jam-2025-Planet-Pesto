using Unity.VisualScripting;
using UnityEngine;

public class DoorPP : NPCPP
{
    [SerializeField] private string activatorEvent;
    [SerializeField] private string connectedScene;

    public void Awake()
    {
        //SetIsInteractable(false);
    }

    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
       if(conversationEndEvent.eventName == activatorEvent)
        {
            SetIsInteractable(true);

            InteractSpriteToggle(true);
        }
    }

    public override void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
        if (!GetIsInteractable())
        {
            InteractSpriteToggle(false);
        }
    }

    public override void Interact()
    {
        Debug.Log("Interacted with door");
        if (GetIsInteractable())
        {
            GameManagerPP.instance.ChangeScene(connectedScene);
        }
    }

    public override void Scan()
    {
        //Ensure it's not null
        if (GetScannerText())
        {
            Talk(GetScannerText());
        }
    }

    public override void StartUp()
    {
        
    }

    public void Talk(DialogueTextPP dialogueText)
    {
        GetDialogueController().DisplayNextInstance(dialogueText);
    }

    
}

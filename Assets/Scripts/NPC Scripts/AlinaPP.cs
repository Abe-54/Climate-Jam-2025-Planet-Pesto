using UnityEngine;
using UnityEngine.InputSystem;

public class AlenaPP : NPCPP, ITalkablePP
{
    [SerializeField] private DialogueTextPP dialogueText;
    [SerializeField] private DialogueTextPP dialogueText1;
    [SerializeField] private DialogueControllerPP dialogueController;

    EventBindingPP<AlinaConversationEvent> alinaConversationEvent;

    private void OnEnable()
    {
        alinaConversationEvent = new EventBindingPP<AlinaConversationEvent>(HandleAlinaConversationEvent);
        EventBusPP<AlinaConversationEvent>.Register(alinaConversationEvent);
    }

    private void OnDisable()
    {
        EventBusPP<AlinaConversationEvent>.Deregister(alinaConversationEvent);
    }

    void HandleAlinaConversationEvent(AlinaConversationEvent conversationEndEvent)
    {
        dialogueText = dialogueText1;
    }

    public override void Interact()
    {

        Talk(dialogueText);
    }
    //Upon the player pressing the interact key, if they are within range then initiate the interaction
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        print("Interact button pressed");
        if (ctx.performed && IsWithinRange())
        {
            Interact();
        }
    }

    public void Talk(DialogueTextPP dialogueText)
    {
        dialogueController.DisplayNextInstance(dialogueText);

    }
}

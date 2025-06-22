using UnityEngine;
using UnityEngine.InputSystem;

public class AlenaPP : NPCPP, ITalkablePP
{
    [SerializeField] private DialogueTextPP dialogueText;
    [SerializeField] private DialogueControllerPP dialogueController;


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

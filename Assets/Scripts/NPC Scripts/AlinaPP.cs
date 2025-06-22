using UnityEngine;

public class AlenaPP : NPCPP, ITalkablePP
{
    [SerializeField] private DialogueTextPP dialogueText;
    [SerializeField] private DialogueControllerPP dialogueController;


    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueTextPP dialogueText)
    {
        dialogueController.DisplayNextInstance(dialogueText);

    }
}

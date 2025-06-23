using System.Runtime.CompilerServices;
using UnityEngine;


[CreateAssetMenu(menuName ="Dialogue/New Dialogue Container")]
public class DialogueTextPP : ScriptableObject
{
   
    public DialogueInstance[] dialogueInstances;
    public bool isEvent;
    public string eventName;


}

[System.Serializable]
public struct DialogueInstance
{
    public string speakerName;

    [TextArea(5, 10)]
    public string[] pharagraphs;
    
}






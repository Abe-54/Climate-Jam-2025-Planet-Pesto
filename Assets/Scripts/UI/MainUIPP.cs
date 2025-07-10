using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPP : MonoBehaviour
{
    [SerializeField] private UIElements dialogueElements;

    public UIElements GetDialogueElements()
    {
        return dialogueElements;
    }


}

[System.Serializable]
public struct UIElements
{
    public TextMeshProUGUI speakerNameTextBox;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueTextBox;
    public Image characterHead;

}
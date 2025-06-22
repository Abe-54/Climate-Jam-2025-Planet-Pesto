
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

//Script for controlling dialogue 
public class DialogueControllerPP : MonoBehaviour
{
    //Fields to control the dialogue UI
    [SerializeField] TextMeshProUGUI speakerName;
    [SerializeField] TextMeshProUGUI dialogue;
    [SerializeField] private float typeSpeed = 10;
    //Tracking whether or not the conversation has ended 
    private bool conEnded = false;
    private bool pharagraphEnded = false;
    private bool isTyping;
    //Queue for the instances of a character speaking
    private Queue<DialogueInstance> dialogueInstances = new Queue<DialogueInstance>();
    //Tracking the pharagraphs for that character 
    private Queue<string> pharagraphs = new Queue<string>();
    private DialogueInstance curInstance;
    private string curPharagraph;




    public void DisplayNextInstance(DialogueTextPP dialogueText)
    {
        //We at the end or start of the dialogue instances?
        if (dialogueInstances.Count == 0 && pharagraphs.Count==0)
        {
            if (!conEnded)
            {
                StartConvo(dialogueText);
            }
            else
            {
                EndConvo();
            }
        }

        if (pharagraphs.Count == 0 && pharagraphEnded)
        {
            curInstance = dialogueInstances.Dequeue();
            speakerName.text = curInstance.speakerName;
            pharagraphEnded=false;
        }

        DisplayNextPharagraph();
    }

    public void DisplayNextPharagraph()
    {
        //Check if that character is done talking
        if (pharagraphs.Count == 0)
        {
            if (!pharagraphEnded)
            {
                StartPharagraph();
            }
            else
            {
                pharagraphEnded = true;
            }
        }

        curPharagraph = pharagraphs.Dequeue();

        dialogue.text = curPharagraph;
        if (pharagraphs.Count == 0)
        {
            pharagraphEnded = true;
        }
           
    }

    private void StartConvo(DialogueTextPP dialogueText)
    {
        //Turn on 
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);    
        }

        //Queue them instances 
        for (int i = 0; i < dialogueText.dialogueInstances.Length; i++)
        {
            dialogueInstances.Enqueue(dialogueText.dialogueInstances[i]);
        }
        curInstance = dialogueInstances.Dequeue();
        speakerName.text = curInstance.speakerName;


    }
    private void EndConvo()
    {

    }

    private void StartPharagraph()
    {
        //Queue them pharagraphs 
        for (int i = 0; i < curInstance.pharagraphs.Length; i++)
        {
            pharagraphs.Enqueue(curInstance.pharagraphs[i]);
        }
    }
    private void EndPharagraph()
    {

    }
    /*
    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        dialogue.text = p;
        dialogue.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in p.ToCharArray())
        {

            maxVisibleChars++;
            dialogue.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }
    */
}

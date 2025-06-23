
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

//Script for controlling dialogue 
public class DialogueControllerPP : MonoBehaviour
{
    //Fields to control the dialogue UI
    [SerializeField] TextMeshProUGUI speakerNameTextBox;
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] private float typeSpeed = 300;
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

    private const float MAX_TYPE_TIME = 1;


    //Courotine for typing out dialogue 
    private Coroutine typeDialogueCoroutine;

    EventBindingPP<ConversationEndEvent> conversationEndEvent;

    private void OnEnable()
    {
        conversationEndEvent = new EventBindingPP<ConversationEndEvent>(HandleConversationEndEvent);
        EventBusPP<ConversationEndEvent>.Register(conversationEndEvent);
    }

    private void OnDisable()
    {
        EventBusPP<ConversationEndEvent>.Deregister(conversationEndEvent);
    }

    void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        Debug.Log("Test event received!");
    }

    //Initiate next pharagraph or speaker or end convo 
    public void DisplayNextInstance(DialogueTextPP dialogueText)
    {
        //We at the end or start of the dialogue instances?
        if (dialogueInstances.Count == 0 && pharagraphs.Count == 0)
        {
            if (!conEnded)
            {
                StartConvo(dialogueText);
            }
            else if (conEnded && !isTyping)
            {
                
                EventBusPP<ConversationEndEvent>.Raise(new ConversationEndEvent());
                EndConvo();
                return;

            }
        }

        if (pharagraphEnded && dialogueInstances.Count != 0 && !isTyping)
        {
            curInstance = dialogueInstances.Dequeue();
            speakerNameTextBox.text = curInstance.speakerName;
            pharagraphEnded = false;
            
        }
        DisplayNextPharagraph();



        if (dialogueInstances.Count == 0)
        {
            conEnded = true;
        }
        
    }

    //Display the next pharagraph 
    public void DisplayNextPharagraph()
    {
        //Check if that character is done talking
        if (pharagraphs.Count == 0)
        {
            if (!pharagraphEnded)
            {
                StartParagraph();
            }
            //Make sure to not stop if it is still typing 
            else if (pharagraphEnded && !isTyping)
            {
                pharagraphEnded = true;
            }
        }

        if (!isTyping )
        {
            curPharagraph = pharagraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(curPharagraph));
        }
        else
        {
            FinishPharagraphEarly();
        }

  
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
        speakerNameTextBox.text = curInstance.speakerName;

    }
    private void EndConvo()
    {
        pharagraphEnded = false;
        conEnded = false;
        gameObject.SetActive(false);
    }

    private void StartParagraph()
    {
        print("Loading in new paragraphs");
        //Queue them pharagraphs 
        for (int i = 0; i < curInstance.pharagraphs.Length; i++)
        {
            pharagraphs.Enqueue(curInstance.pharagraphs[i]);
        }
    }

    //Enumerator 
    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        dialogueTextBox.text = p;
        dialogueTextBox.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in p.ToCharArray())
        {

            maxVisibleChars++;
            dialogueTextBox.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishPharagraphEarly()
    {
        //End corutine 
        StopCoroutine(typeDialogueCoroutine);

        //Finish displaying text
        dialogueTextBox.maxVisibleCharacters = curPharagraph.Length;
        dialogueTextBox.text = curPharagraph;
        print(curPharagraph);

        //Update isTyping
        isTyping = false;
    }
    
}

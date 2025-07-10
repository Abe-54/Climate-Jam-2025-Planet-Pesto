
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;





//Singleton for controlling dialogue 
public class DialogueControllerPP : MonoBehaviour
{

    //Ensure singleton behavior
    public static DialogueControllerPP instance;
    private void Awake()
    {
       
    }

    private void Start()
    {
        //Ensure that this is the only gamemanager in scene
        if (instance == null)
        {
            instance = this;
            dialogueUIElems = FindAnyObjectByType<MainUIPP>().GetDialogueElements();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
           Destroy(this.gameObject);
        }
        

    }

    //Fields to control the dialogue UI
    [SerializeField] UIElements dialogueUIElems;
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

    }

    //Initiate next pharagraph or speaker or end convo 
    public void DisplayNextInstance(DialogueTextPP dialogueText)
    {
        //We at the end or start of the dialogue instances?
        if (dialogueInstances.Count == 0 && pharagraphs.Count == 0)
        {
            if (!conEnded)
            {
                EventBusPP<ConversationStartEvent>.Raise(new ConversationStartEvent { });
                StartConvo(dialogueText);
            }
            else if (conEnded && !isTyping)
            {
                EventBusPP<ConversationEndEvent>.Raise(new ConversationEndEvent
                {
                    eventName = dialogueText.eventName
                });
                EndConvo();
                return;

            }
        }

        if (pharagraphEnded && dialogueInstances.Count != 0 && !isTyping)
        {
            curInstance = dialogueInstances.Dequeue();
            SelectHeadImage();
            SelectFont();
            dialogueUIElems.speakerNameTextBox.text = curInstance.speakerName;
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

        if (!isTyping)
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

    private void OnLevelWasLoaded(int level)
    {
        dialogueUIElems = FindAnyObjectByType<MainUIPP>().GetDialogueElements();
    }
    private void StartConvo(DialogueTextPP dialogueText)
    {

       

        dialogueUIElems.dialogueUI.SetActive(true);
        
        
        

        //Queue them instances 
        for (int i = 0; i < dialogueText.dialogueInstances.Length; i++)
        {
            dialogueInstances.Enqueue(dialogueText.dialogueInstances[i]);
        }
        curInstance = dialogueInstances.Dequeue();
        SelectHeadImage();
        dialogueUIElems.speakerNameTextBox.text = curInstance.speakerName;
        SelectFont();

    }
    private void EndConvo()
    {
        pharagraphEnded = false;
        conEnded = false;
        dialogueUIElems.dialogueUI.SetActive(false);
    }

    private void StartParagraph()
    {
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

        dialogueUIElems.dialogueTextBox.text = p;
        dialogueUIElems.dialogueTextBox.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in p.ToCharArray())
        {

            maxVisibleChars++;
            dialogueUIElems.dialogueTextBox.maxVisibleCharacters = maxVisibleChars;
            AudioManagerPP.instance.PlayDialogueNoise(curInstance.speakerName,maxVisibleChars);

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishPharagraphEarly()
    {
        //End corutine 
        StopCoroutine(typeDialogueCoroutine);

        //Finish displaying text
        dialogueUIElems.dialogueTextBox.maxVisibleCharacters = curPharagraph.Length;
        dialogueUIElems.dialogueTextBox.text = curPharagraph;
       

        //Update isTyping
        isTyping = false;
    }

    private void SelectHeadImage()
    {

        switch (curInstance.speakerName)
        {
            case "I.R.I.S":
                {
                    dialogueUIElems.characterHead.sprite = Resources.Load<Sprite>("Dialogue/Art/HeadShots/IRIS");
                    break;
                }
            default:
                {
                    if (Resources.Load<Sprite>("Dialogue/Art/HeadShots/" + curInstance.speakerName))
                    {
                        dialogueUIElems.characterHead.sprite = Resources.Load<Sprite>("Dialogue/Art/HeadShots/" + curInstance.speakerName);
                    }
                    else
                    {
                        dialogueUIElems.characterHead.sprite = Resources.Load<Sprite>("Dialogue/Art/HeadShots/default");
                    }
                    break;
                }
        }
    }

    private void SelectFont()
    {
        switch (curInstance.speakerName)
        {

            case "I.R.I.S":
                {
                    Debug.Log("got here");
                    dialogueUIElems.dialogueTextBox.font = Resources.Load<TMP_FontAsset>("Dialogue/Fonts/IRIS");
                    break;
                }
            default:
                {
                    if (Resources.Load<TMP_FontAsset>("Dialogue/Fonts" + curInstance.speakerName))
                    {
                        dialogueUIElems.dialogueTextBox.font = Resources.Load<TMP_FontAsset>("Dialogue/Fonts" + curInstance.speakerName);
                    }
                    else
                    {
                        dialogueUIElems.dialogueTextBox.font = Resources.Load<TMP_FontAsset>("Dialogue/Fonts/default");
                    }
                    break;
                }
        }
    }
    
}

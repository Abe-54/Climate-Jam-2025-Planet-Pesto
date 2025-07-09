using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Timeline;

public class CreatureCutsceneTriggerPP : MonoBehaviour
{
    [SerializeField] private TimelineAsset triggeringCutscene;
    [SerializeField] public AudioClip creatureMusic;
    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;

    private void OnEnable()
    {
        conversationEndEvent = new EventBindingPP<ConversationEndEvent>(HandleConversationEndEvent);
        EventBusPP<ConversationEndEvent>.Register(conversationEndEvent);

        conversationStartEvent = new EventBindingPP<ConversationStartEvent>(HandleConversationStartEvent);
        EventBusPP<ConversationStartEvent>.Register(conversationStartEvent);
    }

    private void OnDisable()
    {
        EventBusPP<ConversationEndEvent>.Deregister(conversationEndEvent);
        EventBusPP<ConversationStartEvent>.Deregister(conversationStartEvent);
    }
    public void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
       if(conversationEndEvent.eventName == "THEORB")
       {
            AudioManagerPP.instance.PlayMusic(creatureMusic);
            EventBusPP<CutsceneTrigger>.Raise(new CutsceneTrigger
            {
                cutscene = triggeringCutscene
            });
     
            
        }
     
    }

    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {

    }
    private void Start()
    {
     
    }
}

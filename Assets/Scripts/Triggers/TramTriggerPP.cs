using UnityEngine;
using UnityEngine.Timeline;

public class TrainTriggerPP : MonoBehaviour
{
    [SerializeField] private TimelineAsset triggeringCutscene;
    [SerializeField] private AudioClip tramMusic;
    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;
    private bool TramRight;
    private bool TramLeft;

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

    private void Start()
    {
        AudioManagerPP.instance.PlayMusic(tramMusic);
    }

    public void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        if (conversationEndEvent.eventName == "TramRight")
        {
            TramRight = true;
        }
        if (conversationEndEvent.eventName == "TramLeft")
        {
            TramLeft = true;
        }

        if (TramLeft && TramRight)
        {
            EventBusPP<CutsceneTrigger>.Raise(new CutsceneTrigger
            {
                cutscene = triggeringCutscene
            });
        }
    }

    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {

    }
}

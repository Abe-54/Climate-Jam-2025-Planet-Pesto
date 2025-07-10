using UnityEngine;

public abstract class TriggersAbstractPP : MonoBehaviour
{
    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;
    [SerializeField] private string triggeringEvent;

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
        if (conversationEndEvent.eventName == triggeringEvent)
        {

            Trigger();
        }

    }

    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
    }

    public abstract void Trigger();
}

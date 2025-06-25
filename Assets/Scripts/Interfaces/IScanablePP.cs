using UnityEngine;

public interface IScanablePP 
{
    public void Scan();
    public void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent);
}

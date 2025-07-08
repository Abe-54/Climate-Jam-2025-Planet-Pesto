using UnityEngine;

public class CreaturePP : NPCPP
{
    public override void HandleConversationEndEvent(ConversationEndEvent conversationEndEvent)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public override void Scan()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetIsInteractable(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

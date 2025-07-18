using UnityEngine;

public class FireWallTriggerPP : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // Reference to the spawn point for the firewall, can be set in the inspector or dynamically assigned
    [SerializeField] private GameObject fireWallPrefab; // Reference to the firewall prefab, can be set in the inspector or dynamically assigned
    private bool isFireWallTriggered = false;
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

    }

    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned in the FireWallTriggerPP script.");
        }
        if (collision.gameObject.CompareTag("Player") && !isFireWallTriggered && GameManagerPP.instance.GetCreatureFlag())
        {
            // Trigger the firewall when the player enters the trigger area
            isFireWallTriggered = true;
            Instantiate(fireWallPrefab, new Vector2(spawnPoint.position.x, spawnPoint.position.y), spawnPoint.rotation);
        }
    }
}

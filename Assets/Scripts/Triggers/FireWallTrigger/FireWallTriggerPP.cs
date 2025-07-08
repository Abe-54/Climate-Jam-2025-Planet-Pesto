using UnityEngine;

public class FireWallTriggerPP : MonoBehaviour
{
    [SerializeField] private GameObject fireWallPrefab; // Reference to the firewall prefab, can be set in the inspector or dynamically assigned
    [SerializeField] private float spawnTimer = 1f; // Duration for which the firewall will be spawned between spawns
    [SerializeField] private int maxSpawns = 8; // Maximum number of times the firewall can be spawned 
    private bool isFireWallTriggered = false;
    private float timer = 0f; // Store the initial spawn timer value
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
        if (conversationEndEvent.eventName == "FireWallTriggered")
        {
            isFireWallTriggered = true;
        }
    }

    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFireWallTriggered == true && maxSpawns > 0 && timer < 0)
        {
            timer = spawnTimer; // Reset the timer to the initial value
            maxSpawns--; // Decrease the number of spawns left
            Instantiate(fireWallPrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation);
        }
        else if (timer >= 0)
        {
            timer -= Time.deltaTime; // Decrease the timer by the time passed since last frame
        }
        else if (maxSpawns < 0)
        {
            isFireWallTriggered = false; // Stop spawning firewalls when max spawns are reached
        }
    }
}

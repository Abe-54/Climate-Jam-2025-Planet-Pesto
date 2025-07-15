using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerPP : MonoBehaviour
{

    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;

    [SerializeField]private bool creatureFlag;
    private float intensityLevel = 1;


    [SerializeField]private GameObject playSpawnPos;


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
        if(conversationEndEvent.eventName == "Creature1")
        {
            SetCreatureFlag(true);
        }
    }

    public void HandleConversationStartEvent(ConversationStartEvent conversationStartEvent)
    {

    }


    //To ensure singleton behavior
    public static GameManagerPP instance;

    private void Awake()
    {
        //Ensure that this is the only gamemanager in scene
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        if (!FindAnyObjectByType<PlayerControllerPP>() && playSpawnPos && SceneManager.GetActiveScene().name != "Title Screen")
        {

            Instantiate(Resources.Load("Prefab/Player"), playSpawnPos.transform.position, Quaternion.identity);

        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }

    private void OnLevelWasLoaded(int level)
    {
        if (!FindAnyObjectByType<PlayerControllerPP>()  && playSpawnPos)
        {
            
             Instantiate(Resources.Load("Prefab/Player"), playSpawnPos.transform.position, Quaternion.identity);
            
        }

    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetPlayerSpawn(GameObject spawnPosition)
    {
        playSpawnPos = spawnPosition;
    }

    public bool GetCreatureFlag()
    {
        return creatureFlag;
    }
    public void SetCreatureFlag(bool newBool)
    {
        creatureFlag = newBool; 
    }

    public float GetIntensityLevel()
    {
        return intensityLevel;
    }

    public void SetIntensityLevel(float newIntensityLevel)
    {
        intensityLevel = newIntensityLevel;
    }

    internal void PlayerHitByFirewall()
    {
        Debug.Log("Player was hit by the firewall!");
        // Implement logic for when the player is hit by the firewall
        // This could be game over logic, reducing health, etc.
        // For example, you might want to end the game or reduce the player's health.
        // Example:
        // PlayerControllerPP player = FindAnyObjectByType<PlayerControllerPP>();
        // if (player != null)
        // {
        //     player.TakeDamage(1); // Assuming TakeDamage is a method in PlayerControllerPP
        // }
        // For now, we will just log a message.
        // You can replace this with actual game logic as needed.
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerPP : MonoBehaviour
{

    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;



    private Vector3 playSpawnPos;


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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (!FindAnyObjectByType<PlayerControllerPP>())
        {
            Instantiate(Resources.Load("Prefab/Player"), playSpawnPos, Quaternion.identity);
        }
    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetPlayerSpawn(Vector3 spawnPosition)
    {
        playSpawnPos = spawnPosition;
    }


    
}


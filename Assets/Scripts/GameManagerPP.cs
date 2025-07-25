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

            Instantiate(Resources.Load("Prefab/Player"), playSpawnPos.transform.localToWorldMatrix.GetPosition(), Quaternion.identity);
            Debug.Log(playSpawnPos.transform.localToWorldMatrix.GetPosition());

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

            Instantiate(Resources.Load("Prefab/Player"), playSpawnPos.transform.localToWorldMatrix.GetPosition(), Quaternion.identity);

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

    
}


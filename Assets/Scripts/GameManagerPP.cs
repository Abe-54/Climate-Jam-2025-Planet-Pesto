using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerPP : MonoBehaviour
{

    EventBindingPP<ConversationEndEvent> conversationEndEvent;
    EventBindingPP<ConversationStartEvent> conversationStartEvent;

    [SerializeField]private bool creatureFlag;
    private float intensityLevel = 1;


    [SerializeField]private GameObject playSpawnPos;
    [SerializeField] private GameObject[] playerCheckPoints;
    [SerializeField] private GameObject[] fireWallCheckPoints;
    private GameObject fireWallCurrentCheckpoint; // This is a single checkpoint, not an array
    private GameObject playerCurrentCheckpoint; // This is a single checkpoint, not an array
    private int fireWallCurrentCheckpointIndex = 0; // Index of the current firewall checkpoint

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

    internal void PlayerHitByFirewall()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject fireWall = GameObject.FindGameObjectWithTag("FireWall");
        if (player == null || fireWall == null)
        {
            Debug.LogWarning("Player or FireWall not found!");
        }
        if (fireWallCurrentCheckpoint == null)
        {
            fireWallCurrentCheckpoint = fireWallCheckPoints[0]; // Default to the first checkpoint if none is set
        }
        if (playerCurrentCheckpoint == null)
        {
            playerCurrentCheckpoint = playerCheckPoints[0]; // Default to the first checkpoint if none is set
        }
        player.transform.position = playerCurrentCheckpoint.transform.position; // Reset player position to the current checkpoint
        fireWall.transform.position = fireWallCurrentCheckpoint.transform.position; // Reset firewall position to the current checkpoint
        player.GetComponent<SteamControllerPP>().RemoveSteam(10); // Remove 10 steam units when hit by the firewall
    }

    internal void SetPlayerCurrentCheckpoint(GameObject gameObject, int checkpointIndex)
    {
        playerCurrentCheckpoint = gameObject; // Set the player's current checkpoint to the provided game object
        fireWallCurrentCheckpointIndex = checkpointIndex; // Set the index of the current firewall checkpoint
        fireWallCurrentCheckpoint = fireWallCheckPoints[checkpointIndex]; // Set the current firewall checkpoint based on the index
        Debug.Log("Player has reached a checkpoint: " + gameObject.name + " with index: " + checkpointIndex);
    }
}


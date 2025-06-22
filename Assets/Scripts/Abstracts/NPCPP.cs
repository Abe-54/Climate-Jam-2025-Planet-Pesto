using UnityEngine;
using UnityEngine.InputSystem;

//An abstract class which defines the abilities of NPC's within the game
public abstract class NPCPP : MonoBehaviour, IIInteractablePP
{
    //Sprite to indicate that something is interactable
    [SerializeField] private SpriteRenderer interactSprite;
    //Variable to keep track of a players location
    private Transform playerTrans;

    private const float INTERACT_RANGE = 5f;
    
    private void Start()
    {
        //Grab player location
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //Abstract method for interacting with something
    public abstract void Interact();

    // Update is called once per frame
    void Update()
    {
        //If the player presses the interact button and is within a specific range of the npc, interact
        /*
        if (Keyboard.current.eKey.wasPressedThisFrame && IsWithinRange())
        {
            //Interact
            Interact();
        }
        */
        
        //Ensure the intract sprite is not active when out of range
        if (interactSprite.gameObject.activeSelf && !IsWithinRange())
        {
            interactSprite.gameObject.SetActive(false);
        }

        if (!interactSprite.gameObject.activeSelf && IsWithinRange())
        {
            interactSprite.gameObject.SetActive(true);
        }
    }



    //Method to keep track of whether or not the player is within range to interact
    public bool IsWithinRange()
    {
        if (Vector2.Distance(transform.position, playerTrans.position) < INTERACT_RANGE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
}

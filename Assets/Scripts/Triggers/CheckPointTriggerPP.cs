using UnityEngine;

public class CheckPointTriggerPP : MonoBehaviour
{
    [SerializeField] private int checkpointIndex = 0; // Index of the checkpoint, can be set in the inspector or dynamically assigned

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set the player's current checkpoint to this checkpoint
            GameManagerPP.instance.SetPlayerCurrentCheckpoint(gameObject, checkpointIndex);
        }
    }
}

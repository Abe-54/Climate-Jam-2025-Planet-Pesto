using UnityEngine;

public class DialogueCollider : MonoBehaviour
{
    private NPCPP NPC;
    private void Start()
    {
        NPC = GetComponentInParent<NPCPP>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

        }
    }

    public NPCPP GetNPC()
    {
        return NPC;
    }

    



}

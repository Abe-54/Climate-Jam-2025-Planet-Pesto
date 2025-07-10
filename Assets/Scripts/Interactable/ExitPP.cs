
using UnityEngine;

public class ExitPP : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [SerializeField] private Vector3 playSpawnPos;

    private void Start()
    {
        
        if (GameManagerPP.instance.GetCreatureFlag())
        {
            sceneName = "Lab2";
            playSpawnPos = new Vector3(-50, -5, 0);
        }
        else
        {
            sceneName = "Creature";
            playSpawnPos = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManagerPP.instance.SetPlayerSpawn(playSpawnPos);
            GameManagerPP.instance.ChangeScene(sceneName);  
        }
    }
}

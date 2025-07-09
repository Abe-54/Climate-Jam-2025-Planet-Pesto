using UnityEngine;

public class ExitPP : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector3 playSpawnPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManagerPP.instance.SetPlayerSpawn(playSpawnPos);
            GameManagerPP.instance.ChangeScene(sceneName);  
        }
    }
}

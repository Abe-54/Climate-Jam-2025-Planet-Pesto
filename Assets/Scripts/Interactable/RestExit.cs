using UnityEngine;

public class RestExit : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject playSpawnPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManagerPP.instance.SetPlayerSpawn(playSpawnPos);
            
            GameManagerPP.instance.SetIntensityLevel(2);
            FindFirstObjectByType<SteamControllerPP>().AddSteam(100);


            GameManagerPP.instance.ChangeScene(sceneName);
        }
    }
}

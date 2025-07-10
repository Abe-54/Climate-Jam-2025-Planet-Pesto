
using UnityEngine;

public class ExitPP : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [SerializeField] private GameObject playSpawnPos;

    private void Start()
    {
        
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManagerPP.instance.SetPlayerSpawn(playSpawnPos);
            GameManagerPP.instance.ChangeScene(sceneName);  
        }
    }

    public void SetSceneName(string newSceneName)
    {
        sceneName = newSceneName;
    }
    public void SetSpawn(GameObject newSpawnPos)
    {
        playSpawnPos = newSpawnPos;
    }
}

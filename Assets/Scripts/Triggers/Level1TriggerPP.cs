using UnityEngine;

public class Level1TriggerPP : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private GameObject entrance;
    [SerializeField] private GameObject exit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<PlayerControllerPP>().SetInfiniteDash(false);
        AudioManagerPP.instance.PlayMusic(levelMusic);

        if (GameManagerPP.instance.GetCreatureFlag())
        {
            entrance.SetActive(true);
        }
        else
        {
            exit.SetActive(true);
        }
        
    }

    
}

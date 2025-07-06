using Unity.VisualScripting;
using UnityEngine;

public class AudioManagerPP : MonoBehaviour
{
    //Singleton behavior 
    public static AudioManagerPP instance;

    //Audiosources 
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource dialogueSource;
    [SerializeField] private AudioSource cutsceneSFX;

    [SerializeField] private CharacterAudioInfoPP curCharacter;
    
    private void Awake()
    {
        //Ensure that this is the only audiomanager in scene
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!curCharacter)
        {
            curCharacter = Resources.Load<CharacterAudioInfoPP>("Dialogue/CharacterAudioInfo/default");
        }
    }

    public void PlaySFXClip(AudioClip audioClip,Transform spawnTransform, float volume)
    {
        //Spawn in game object
        AudioSource audioSource =  Instantiate(sfxSource,spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayCinamaticSFXClip(AudioClip audioClip, float volume)
    {
      

        cutsceneSFX.clip = audioClip;

        cutsceneSFX.volume = volume;

        cutsceneSFX.Play();
       
    }



    public void PlayMusic(AudioClip audioClip)
    {
        if (musicSource.isPlaying)
        {
            StopMusic();
        }
        musicSource.clip = audioClip;
        musicSource.Play();
    }

    public void PlayDialogueNoise(string speakerName, float displayCount)
    {
        if(curCharacter.name != speakerName)
        {
          
            SwitchCurCharacter(speakerName); 
        }

        if (dialogueSource.isPlaying)
        {
           dialogueSource.Stop();
        }
        //Ensure the audio is only played at the characters frequency level
        if (displayCount % curCharacter.frequencyLevel == 0)
        {
            //Randomizing clip and pitch
            int randomIndex = Random.Range(0, curCharacter.dialogueTypingSoundClips.Length);
            dialogueSource.clip = curCharacter.dialogueTypingSoundClips[randomIndex];
            dialogueSource.pitch = Random.Range(curCharacter.minPitch, curCharacter.maxPitch);
            dialogueSource.Play();
        }

    }

    public void SwitchCurCharacter(string speakerName)
    {
        switch (speakerName)
        {
            case "I.R.I.S":
                {
                    curCharacter = Resources.Load<CharacterAudioInfoPP>("Dialogue/CharacterAudioInfo/IRIS");
                    break;
                }
            default:
                {
                    curCharacter = Resources.Load<CharacterAudioInfoPP>("Dialogue/CharacterAudioInfo/" + speakerName);
                    break;
                }
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }


 
    
}


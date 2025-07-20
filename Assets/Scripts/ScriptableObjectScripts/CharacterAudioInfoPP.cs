using UnityEngine;


//Scriptable object which contains settings and clips for  character audio
[CreateAssetMenu(menuName = "Audio/New CharacterAudioInfo")]
public class CharacterAudioInfoPP : ScriptableObject
{
    public string name;
    public AudioClip[] dialogueTypingSoundClips;
   
    public int frequencyLevel = 2;
    [Range(-3, 3)]
    public float minPitch = 0.5f;
    [Range(-3, 3)]
    public float maxPitch = 3f;
    [Range(1, 10)]
    public float volume;
    public bool stopAudioSource;
    public bool letFinish;
    

}

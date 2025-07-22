using UnityEngine;

public class PlayerAudioControllerPP : MonoBehaviour
{
    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioSource jumpingSource;
    [SerializeField] private AudioSource dashingSource;
    //Clips for the audio
    [SerializeField] private AudioClip[] walkingSFX;
    [SerializeField] private AudioClip[] jumpingSFX;
    [SerializeField] private AudioClip[] dashingSFX;




    public void PlayMovementSFX(PlayerSFXType type)
    {
        switch (type)
        {
            case PlayerSFXType.walking:
                AudioManagerPP.instance.PlaySFXClipRandom(walkingSFX, this.transform, 1,true,walkingSource);
                break;
            case PlayerSFXType.jumping:
                AudioManagerPP.instance.PlaySFXClipRandom(jumpingSFX, this.transform, 1, true, jumpingSource);
                break;
            case PlayerSFXType.dashing:
                AudioManagerPP.instance.PlaySFXClipRandom(dashingSFX, this.transform, 1, true, dashingSource);
                break;
        }
    }
}

public enum PlayerSFXType
{
    walking,
    jumping,
    dashing
}
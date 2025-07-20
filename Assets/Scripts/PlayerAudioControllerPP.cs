using UnityEngine;

public class PlayerAudioControllerPP : MonoBehaviour
{
    [SerializeField] private AudioClip[] walkingSFX;
    [SerializeField] private AudioClip[] jumpingSFX;
    [SerializeField] private AudioClip[] dashingSFX;


    public void PlayMovementSFX(PlayerSFXType type)
    {
        switch (type)
        {
            case PlayerSFXType.walking:
                AudioManagerPP.instance.PlaySFXClipRandom(walkingSFX, this.transform, 1);
                break;
            case PlayerSFXType.jumping:
                AudioManagerPP.instance.PlaySFXClipRandom(jumpingSFX, this.transform, 1);
                break;
            case PlayerSFXType.dashing:
                AudioManagerPP.instance.PlaySFXClipRandom(dashingSFX, this.transform, 1);
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
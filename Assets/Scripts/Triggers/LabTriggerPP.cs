using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class LabTriggerPP : TriggersAbstractPP
{

    [SerializeField] private TimelineAsset triggeringCutscene;
    [SerializeField] private TimelineAsset StartingCutscene;
    [SerializeField] private bool IntroCutscene;
    [SerializeField] private bool infiniteDash;
    [SerializeField] private AudioClip labMusic;
    public override void Trigger()
    {
        EventBusPP<CutsceneTrigger>.Raise(new CutsceneTrigger
        {
            cutscene = triggeringCutscene
        });

    }
    public void Start()
    {
        if (IntroCutscene)
        {
            EventBusPP<CutsceneTrigger>.Raise(new CutsceneTrigger
            {
                cutscene = StartingCutscene
            });
        }
        if(SceneManager.GetActiveScene().name == "Lab2")
        {
            AudioManagerPP.instance.PlayMusic(labMusic);
        }
        FindAnyObjectByType<PlayerControllerPP>().SetInfiniteDash(infiniteDash);
        
    }
}

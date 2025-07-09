using UnityEngine;
using UnityEngine.Timeline;

public class LabTriggerPP : TriggersAbstractPP
{

    [SerializeField] private TimelineAsset triggeringCutscene;
    [SerializeField] private TimelineAsset StartingCutscene;
    [SerializeField] private bool IntroCutscene;
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
    }
}

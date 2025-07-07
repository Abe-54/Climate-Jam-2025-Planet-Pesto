
using UnityEngine;
using UnityEngine.Playables;


public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playbackControls;

    EventBindingPP<CutsceneTrigger> cutsceneTriggerEvent;

    private void OnEnable()
    {
        cutsceneTriggerEvent = new EventBindingPP<CutsceneTrigger>(HandleCutsceneTriggerEvent);
        EventBusPP<CutsceneTrigger>.Register(cutsceneTriggerEvent);

     
    }

    private void OnDisable()
    {
        EventBusPP<CutsceneTrigger>.Deregister(cutsceneTriggerEvent);
    }

    private void HandleCutsceneTriggerEvent(CutsceneTrigger trigger)
    {
        playbackControls.Play(trigger.cutscene);
    }

  


}

using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;

    private void Start()
    {
        //Get the camera if its not in the slot
        if(!_camera)
        {
            _camera = FindAnyObjectByType<CinemachineCamera>();
        }
        if (!_camera.Target.TrackingTarget)
        {
            _camera.Target.TrackingTarget = FindFirstObjectByType<PlayerControllerPP>().gameObject.transform;
        }
    }

}

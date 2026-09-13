using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachinePanTilt _panTilt;
    [SerializeField] private CinemachineInputAxisController _cameraInput;
    public float PanAxis => _panTilt.PanAxis.Value;
    public void SetCameraInputEnabled(bool isActive)
    {
        _cameraInput.enabled = isActive;
    }
    public void ResetCameraRotation()
    {
        _panTilt.PanAxis.Value = 0;
        _panTilt.TiltAxis.Value = 0;
    }
    public void SetPanAxisValue(float panValue)
    {
        _panTilt.PanAxis.Value = panValue;
    }
    public void SetTiltAxisValue(float tiltValue)
    {
        _panTilt.TiltAxis.Value = tiltValue;
    }
}

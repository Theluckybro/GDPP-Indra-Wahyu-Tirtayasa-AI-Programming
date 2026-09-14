using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _owner;
    [SerializeField] private float _detectorDistance;
    [SerializeField] private Vector3 _detectorBoxSize = Vector3.one;
    [SerializeField] private LayerMask _interactableLayer;
    private IInteractable _detectedInteractable;
    private bool _isInteracting;
    public bool Enabled { get; private set; } = true;

    public void SetEnabled(bool isEnabled)
    {
        Enabled = isEnabled;
    }
    private void Update()
    {
        UpdateDetection();
    }
    private void UpdateDetection()
    {
        if (_isInteracting)
        {
            _isInteracting = false;
            return;
        }
        if (Enabled == true)
        {
            Transform cameraTransform = Camera.main.transform;
            bool isDetectingInteractable = Physics.BoxCast(cameraTransform.position, _detectorBoxSize * 0.5f, cameraTransform.forward, out RaycastHit hit, Quaternion.identity, _detectorDistance, _interactableLayer);
            if (isDetectingInteractable)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    _detectedInteractable = interactable;
                    HUDManager.Instance.CrosshairUI.SetHighlight(true);
                }
                HUDManager.Instance.InteractionInfoUI.SetNameText(_detectedInteractable.Name);
                HUDManager.Instance.InteractionInfoUI.SetVisible(true);
            }
            else
            {
                HUDManager.Instance.InteractionInfoUI.SetVisible(false);
                HUDManager.Instance.CrosshairUI.SetHighlight(false);
            }
        }
        
    }
    public void Interact()
    {
        if (_detectedInteractable != null && Enabled == true)
        {
            _detectedInteractable.Interact(_owner);
            _detectedInteractable = null;
            _isInteracting = true;
            HUDManager.Instance.InteractionInfoUI.SetVisible(false);
            HUDManager.Instance.CrosshairUI.SetHighlight(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Transform cameraTransform = Camera.main.transform;
        if (Enabled == true)
        {
            bool isDetectingInteractable = Physics.BoxCast(cameraTransform.position, _detectorBoxSize * 0.5f, cameraTransform.forward, out RaycastHit hit, Quaternion.identity, _detectorDistance, _interactableLayer);
            if (isDetectingInteractable)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * hit.distance);
                Gizmos.DrawWireCube(cameraTransform.position + cameraTransform.forward * hit.distance, _detectorBoxSize);
            }
            else
            {
                Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * _detectorDistance);
                Gizmos.DrawWireCube(cameraTransform.position + cameraTransform.forward * _detectorDistance, _detectorBoxSize);
            }
        }
    }
}

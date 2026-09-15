using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _owner;
    [SerializeField] private float _detectorDistance;
    // Keep this well below a doorway's width (1m), otherwise an open door is detected while looking through its empty doorway
    [SerializeField] private Vector3 _detectorBoxSize = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private LayerMask _obstacleLayer;
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
            _detectedInteractable = FindInteractable();
            if (_detectedInteractable != null)
            {
                HUDManager.Instance.InteractionInfoUI.SetNameText(_detectedInteractable.Name);
                HUDManager.Instance.InteractionInfoUI.SetVisible(true);
                HUDManager.Instance.CrosshairUI.SetHighlight(true);
            }
            else
            {
                HUDManager.Instance.InteractionInfoUI.SetVisible(false);
                HUDManager.Instance.CrosshairUI.SetHighlight(false);
            }
        }
    }
    private IInteractable FindInteractable()
    {
        Transform cameraTransform = Camera.main.transform;
        float maxDistance = _detectorDistance;
        // Interactables behind a wall must not be reachable
        bool isBlocked = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit obstacleHit, _detectorDistance, _obstacleLayer, QueryTriggerInteraction.Ignore);
        if (isBlocked == true)
        {
            maxDistance = obstacleHit.distance;
        }
        // The box follows the camera rotation, a world-aligned box gets wider on its diagonal when the camera turns
        RaycastHit[] hits = Physics.BoxCastAll(cameraTransform.position, _detectorBoxSize * 0.5f, cameraTransform.forward, cameraTransform.rotation, maxDistance, _interactableLayer);
        IInteractable bestInteractable = null;
        bool isBestPickable = false;
        float bestDistance = float.MaxValue;
        foreach (RaycastHit hit in hits)
        {
            // Colliders overlapping the box at the start of the cast have distance 0, same as BoxCast ignoring them
            if (hit.distance <= 0f)
            {
                continue;
            }
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null)
            {
                continue;
            }
            // Pickable items win over doors/drawers, so a key inside an open drawer can be picked up
            bool isPickable = interactable is IPickable;
            bool isBetter = isPickable != isBestPickable ? isPickable : hit.distance < bestDistance;
            if (bestInteractable == null || isBetter == true)
            {
                bestInteractable = interactable;
                isBestPickable = isPickable;
                bestDistance = hit.distance;
            }
        }
        return bestInteractable;
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
            bool isDetectingInteractable = Physics.BoxCast(cameraTransform.position, _detectorBoxSize * 0.5f, cameraTransform.forward, out RaycastHit hit, cameraTransform.rotation, _detectorDistance, _interactableLayer);
            float gizmoDistance = isDetectingInteractable ? hit.distance : _detectorDistance;
            if (isDetectingInteractable)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * gizmoDistance);
            Gizmos.matrix = Matrix4x4.TRS(cameraTransform.position + cameraTransform.forward * gizmoDistance, cameraTransform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _detectorBoxSize);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}

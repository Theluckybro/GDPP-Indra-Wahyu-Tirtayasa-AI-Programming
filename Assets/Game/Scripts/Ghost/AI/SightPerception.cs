using UnityEngine;

public class SightPerception : MonoBehaviour
{
    [SerializeField] private Transform _eyePosition;
    [SerializeField] private Transform _target;
    [SerializeField] private float _viewDistance = 10f; 
    [SerializeField] private float _viewAngle = 70f;
    [SerializeField] private LayerMask _targetLayer;
    // The target pivot is at the player's feet; aim at the body instead
    [SerializeField] private float _targetHeightOffset = 1f;
    public bool CanSeePlayer { get; private set; }
    public Vector3 LastSeenPosition { get; private set; }
    private void Update()
    {
        CanSeePlayer = CheckSight();
    }
    public bool CheckSight()
    {
        if (_target == null)
        {
            return false;
        }
        Vector3 targetPosition = _target.position + Vector3.up * _targetHeightOffset;
        float distance = Vector3.Distance(_eyePosition.position, targetPosition);
        if (distance > _viewDistance)
        {
            return false;
        }

        Vector3 dirToTarget = targetPosition - _eyePosition.position;
        float angle = Vector3.Angle(_eyePosition.forward, dirToTarget);
        if (angle > _viewAngle * 0.5f)
        {
            return false;
        }

        bool isSeeTarget = Physics.Raycast(_eyePosition.position, dirToTarget, out RaycastHit hit, _viewDistance, _targetLayer);
        if (isSeeTarget)
        {
            if (hit.transform == _target)
            {
                LastSeenPosition = _target.position;
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        if (_eyePosition == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        bool isSeeTarget = CheckSight();
        if (isSeeTarget)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(_eyePosition.position, _viewDistance);
        Vector3 left = Quaternion.Euler(0, -_viewAngle / 2, 0) * _eyePosition.forward;
        Vector3 right = Quaternion.Euler(0, _viewAngle / 2, 0) * _eyePosition.forward;
        Gizmos.DrawRay(_eyePosition.position, left * _viewDistance);
        Gizmos.DrawRay(_eyePosition.position, right * _viewDistance);
    }
}

using UnityEngine;
public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    private Vector3 _movementDirection;
    private float _currentSpeed = 1f;
    private Vector3 _velocityXZ;
    public void SetMoveDirection(Vector2 moveDirection)
    {
        _movementDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
    }
    private void CalculateVelocityXZ()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 xDirection = _movementDirection.x * cameraTransform.right;
        Vector3 zDirection = _movementDirection.z * cameraTransform.forward;
        Vector3 direction = xDirection + zDirection;
        direction.y = 0;
        if (_movementDirection.magnitude > 0.01)
        {
            _velocityXZ = direction.normalized * _currentSpeed * Time.deltaTime;
        }
        else
        {
            _velocityXZ = Vector3.zero;
        }
    }
    public void Move()
    {
        CalculateVelocityXZ();
        _characterController.Move(_velocityXZ);
    }
    private void Update()
    {
        Move();
    }
}

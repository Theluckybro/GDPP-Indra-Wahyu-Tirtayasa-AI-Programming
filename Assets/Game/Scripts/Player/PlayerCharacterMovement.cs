using UnityEngine;
public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _gravityScale = 1;
    private Vector3 _movementDirection;
    private float _currentSpeed = 1f;
    private Vector3 _velocityXZ;
    private float _velocityY;
    private bool _isGrounded;
    private void CheckIsGrounded()
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        _isGrounded = Physics.CheckSphere(transform.position, 0.5f, groundLayer);
    }
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
    private void CalculateVelocityY()
    {
        _velocityY = _velocityY + Physics.gravity.y * _gravityScale * Time.deltaTime;
    }
    public void Move()
    {
        CalculateVelocityXZ();
        CalculateVelocityY();
        Vector3 velocity = new Vector3(_velocityXZ.x, _velocityY, _velocityXZ.z);
        _characterController.Move(velocity);
    }
    private void ResetVelocityY()
    {
        if (_isGrounded && _velocityY < 0)
        {
            _velocityY = -2;
        }
    }
    private void Update()
    {
        CheckIsGrounded();
        ResetVelocityY();
        Move();
    }
}

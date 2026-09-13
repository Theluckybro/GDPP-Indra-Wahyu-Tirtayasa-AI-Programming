using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCharacterMovement _movement;
    [SerializeField] private PlayerCharacterStamina _stamina;
    [SerializeField] private InventoryManager _inventory;
    [SerializeField] private InteractDetector _interactDetector;
    [SerializeField] private CameraManager _camera;
    [SerializeField] private InputManager _input;
    public PlayerCharacterMovement Movement => _movement;
    public PlayerCharacterStamina Stamina => _stamina;
    public InventoryManager Inventory => _inventory;
    public InteractDetector InteractDetector => _interactDetector;
    public CameraManager Camera => _camera;
    public InputManager Input => _input;
    public bool IsHiding { get; private set; }
    public void SetHiding(bool isHiding)
    {
        IsHiding = isHiding;
    }
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

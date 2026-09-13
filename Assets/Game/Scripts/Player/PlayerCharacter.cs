using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCharacterMovement _movement;
    [SerializeField] private PlayerCharacterStamina _stamina;
    [SerializeField] private InventoryManager _inventory;
    [SerializeField] private InteractDetector _interactDetector;
    public PlayerCharacterMovement Movement => _movement;
    public PlayerCharacterStamina Stamina => _stamina;
    public InventoryManager Inventory => _inventory;
    public InteractDetector InteractDetector => _interactDetector;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

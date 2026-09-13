using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCharacterMovement _movement;
    [SerializeField] private PlayerCharacterStamina _stamina;
    [SerializeField] private InventoryManager _inventory;
    public PlayerCharacterMovement Movement => _movement;
    public PlayerCharacterStamina Stamina => _stamina;
    public InventoryManager Inventory => _inventory;
}

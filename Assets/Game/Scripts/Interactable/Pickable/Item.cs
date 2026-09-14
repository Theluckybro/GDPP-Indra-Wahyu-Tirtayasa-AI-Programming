using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] private ItemData _itemData;

    public string Name => _itemData.Name;
    public UnityEvent OnItemPicked;

    [ContextMenu("Interact Item")]
    public void Interact(PlayerCharacter character)
    {
        Pickup(character);
    }

    public virtual void Pickup(PlayerCharacter character)
    {
        ItemData newData = new ItemData(_itemData.ID, _itemData.Name);
        character.Inventory.AddItems(newData);
        OnItemPicked?.Invoke();
        Destroy(gameObject);
    }
}

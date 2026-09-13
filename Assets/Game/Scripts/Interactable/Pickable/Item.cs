using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] private ItemData _itemData;

    public string Name => _itemData.Name;
    public UnityEvent OnItemPicked;

    [ContextMenu("Interact Item")]
    public void Interact()
    {
        Pickup();
    }

    public void Pickup()
    {
        OnItemPicked?.Invoke();
        Destroy(gameObject);
    }
}

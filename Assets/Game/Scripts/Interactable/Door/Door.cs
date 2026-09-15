using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _name;
    [SerializeField] protected Transform _doorTransform;
    [SerializeField] protected float _duration = 1f;
    [SerializeField] protected bool _islocked;
    [SerializeField] protected string _keyID;
    protected bool _isAnimating;
    protected bool _isOpen;
    protected Coroutine _animatingDoorCoroutine;
    public bool IsAnimating => _isAnimating;
    public string Name => _name;
    public UnityEvent OnDoorOpen;
    public UnityEvent OnDoorClose;
    public UnityEvent OnOpenLockedDoor;
    [ContextMenu("Interact Door")]
    public void Interact(PlayerCharacter character)
    {
        if (_islocked == true)
        {
            bool hasKey = character.Inventory.CheckItem(_keyID);
            if (hasKey)
            {
                _islocked = false;
                Open();
            }
            else
            {
                OnOpenLockedDoor?.Invoke();
            }
        }
        else
        {
            if (_isOpen == true)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
    public virtual void Open()
    {
        _isOpen = true;
        OnDoorOpen?.Invoke();
    }
    public virtual void Close()
    {
        _isOpen = false;
        OnDoorClose?.Invoke();
    }
}

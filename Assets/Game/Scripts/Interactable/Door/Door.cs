using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _name;
    [SerializeField] protected Transform _doorTransform;
    // Optional, when left empty it falls back to the collider on the door transform
    [SerializeField] protected Collider _doorCollider;
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
    protected virtual void Awake()
    {
        if (_doorCollider == null && _doorTransform != null)
        {
            _doorCollider = _doorTransform.GetComponent<Collider>();
        }
        UpdateCollision();
    }
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
        UpdateCollision();
        OnDoorOpen?.Invoke();
    }
    public virtual void Close()
    {
        _isOpen = false;
        UpdateCollision();
        OnDoorClose?.Invoke();
    }
    // An open door turns into a trigger instead of being disabled, so the player walks through it while the interaction box cast still finds it and can close it again
    protected void UpdateCollision()
    {
        if (_doorCollider != null)
        {
            _doorCollider.isTrigger = _isOpen;
        }
    }
}

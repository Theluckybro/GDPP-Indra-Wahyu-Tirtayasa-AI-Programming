using UnityEngine;
using UnityEngine.Events;

public class GameEventBase : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private bool _isOneTime;
    public UnityEvent OnEventTriggered;
    public UnityEvent OnEventFinished;
    public string ID => _id;
    public void Start()
    {
        GameEventManager.Instance.Register(this);
    }
    public virtual void Trigger()
    {
        OnEventTriggered?.Invoke();
    }
    public virtual void Finish()
    {
        OnEventFinished?.Invoke();
        if (_isOneTime == true)
        {
            GameEventManager.Instance.Unregister(this);
            Destroy(gameObject);
        }
    }
}

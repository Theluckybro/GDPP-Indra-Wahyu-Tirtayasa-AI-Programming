using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GhostAIController : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent _behaviorGraphAgent;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private PlayerCharacter _target;
    [SerializeField] private SightPerception _sightPerception;
    public UnityEvent OnDespawn;
    public BehaviorGraphAgent BehaviorGraphAgent => _behaviorGraphAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public PlayerCharacter Target => _target;
    public SightPerception SightPerception => _sightPerception;
    public void Despawn()
    {
        StartCoroutine(DespawnAfterEndOfFrame());
    }
    private IEnumerator DespawnAfterEndOfFrame()
    {
        if (_behaviorGraphAgent != null)
        {
            _behaviorGraphAgent.SetVariableValue("CanSeeTarget", false);
            _behaviorGraphAgent.enabled = false;
        }
        if (_navMeshAgent != null && _navMeshAgent.isOnNavMesh == true)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.enabled = false;
        }
        OnDespawn?.Invoke();
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter character = other.GetComponent<PlayerCharacter>();
            if (character != null)
            {
                character.Death();
            }
        }
    }
}

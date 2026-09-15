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
    // The Navigate node stops ~1.3m from the player's collider, so the trigger collider alone never touches the player
    [SerializeField] private float _catchDistance = 1.8f;
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
        // Wait until the behavior graph has finished its tick before disabling it
        yield return new WaitForEndOfFrame();
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
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (_target == null || _target.IsHiding == true || _sightPerception == null)
        {
            return;
        }
        if (_sightPerception.CanSeePlayer == false)
        {
            return;
        }
        Vector3 offset = _target.transform.position - transform.position;
        offset.y = 0;
        if (offset.magnitude <= _catchDistance)
        {
            _target.Death();
        }
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

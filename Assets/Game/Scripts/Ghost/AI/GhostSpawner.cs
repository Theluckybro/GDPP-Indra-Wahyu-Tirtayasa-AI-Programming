using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private GhostAIController _aiController;
    [SerializeField] private float _minSpawnDelay = 5f;
    [SerializeField] private float _maxSpawnDelay = 8f;
    [SerializeField] private float _minSpawnDistance = 3f;
    [SerializeField] private float _maxSpawnDistance = 5f;
    private Coroutine _spawnCoroutine;
    public void RestartSpawn()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
        _spawnCoroutine = StartCoroutine(StartSpawn());
    }
    public IEnumerator StartSpawn()
    {
        float spawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        yield return new WaitForSeconds(spawnDelay);
        if (_aiController.Target == null || _aiController.Target.IsHiding == true)
        {
            RestartSpawn();
            yield break;
        }
        SpawnGhost();
    }
    public void SpawnGhost()
    {
        bool isSpawnPosFound = TryGetSpawnPosition(out Vector3 spawnPos);
        if (isSpawnPosFound == false)
        {
            RestartSpawn();
            return;
        }
        // The agent can only be warped while its GameObject is active
        _aiController.gameObject.SetActive(true);
        _aiController.NavMeshAgent.enabled = true;
        _aiController.NavMeshAgent.Warp(spawnPos);
        Vector3 lookPos = _aiController.Target.transform.position;
        lookPos.y = spawnPos.y;
        _aiController.transform.LookAt(lookPos);
        _aiController.BehaviorGraphAgent.SetVariableValue("LastSeenPosition", _aiController.Target.transform.position);
        _aiController.BehaviorGraphAgent.enabled = true;
    }
    private bool TryGetSpawnPosition(out Vector3 spawnPos)
    {
        Transform target = _aiController.Target.transform;
        // Spawn behind where the player is looking; the player transform itself never rotates with the camera
        Vector3 backward = -Camera.main.transform.forward;
        backward.y = 0;
        if (backward.sqrMagnitude < 0.01f)
        {
            backward = -target.forward;
        }
        backward.Normalize();
        for (int i = 0; i < 8; i++)
        {
            // Try straight behind first, then fan out to the sides
            float angle = (i + 1) / 2 * 45f * (i % 2 == 0 ? 1 : -1);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * backward;
            float spawnDistance = Random.Range(_minSpawnDistance, _maxSpawnDistance);
            Vector3 candidate = target.position + direction * spawnDistance;
            bool isOnNavMesh = NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1f, NavMesh.AllAreas);
            if (isOnNavMesh == false)
            {
                continue;
            }
            // Stay on the player's floor and don't snap right next to the player
            bool isSameFloor = Mathf.Abs(hit.position.y - target.position.y) < 1f;
            bool isFarEnough = Vector3.Distance(hit.position, target.position) >= _minSpawnDistance;
            if (isSameFloor == true && isFarEnough == true)
            {
                spawnPos = hit.position;
                return true;
            }
        }
        spawnPos = Vector3.zero;
        return false;
    }
}

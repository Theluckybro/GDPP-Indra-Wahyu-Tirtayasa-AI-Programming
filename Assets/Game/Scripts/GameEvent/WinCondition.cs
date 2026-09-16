using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private DisplayCursor _displayCursor;
    [SerializeField] private string _winScene = "WinScreen";
    // Gives the exit door time to swing open and play its audio before the scene swaps
    [SerializeField] private float _delay = 1.5f;
    public UnityEvent OnWin;
    private bool _hasWon;
    [ContextMenu("Win")]
    public void Win()
    {
        // The exit door stays interactable after it opens, the win only counts once
        if (_hasWon == true)
        {
            return;
        }
        _hasWon = true;
        StopGhost();
        OnWin?.Invoke();
        StartCoroutine(LoadWinSceneAfterDelay());
    }
    private IEnumerator LoadWinSceneAfterDelay()
    {
        yield return new WaitForSeconds(_delay);
        if (_displayCursor != null)
        {
            _displayCursor.ShowCursor();
        }
        if (_sceneLoader != null)
        {
            _sceneLoader.LoadScene(_winScene);
        }
        else
        {
            SceneManager.LoadScene(_winScene);
        }
    }
    // The ghost keeps hunting while the win scene is still loading, stopping it prevents a lose screen on top of the win
    private void StopGhost()
    {
        GhostAIController ghost = FindFirstObjectByType<GhostAIController>();
        if (ghost != null && ghost.gameObject.activeInHierarchy == true)
        {
            ghost.Despawn();
        }
        GhostSpawner spawner = FindFirstObjectByType<GhostSpawner>();
        if (spawner != null)
        {
            spawner.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Botones del Menú")]
    public Button bcontinue;
    public Button restart;
    public Button quit;

    private void Start()
    {
        if (bcontinue != null)
        {
            bcontinue.onClick.AddListener(OnContinueClicked);
        }

        if (restart != null)
        {
            restart.onClick.AddListener(OnRestartClicked);
        }

        if (quit != null)
        {
            quit.onClick.AddListener(OnQuitClicked);
        }
    }

    public void OnContinueClicked()
    {
        Debug.Log("[GameOverUI] Click en Continue");
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log($"[GameOverUI] Player: {player}, Manager: {CheckpointManager.Instance}");

        if (player != null && CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RespawnPlayer(player);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitClicked()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject victoryPanel;

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void CheckEnemiesAlive()
    {
        Invoke(nameof(EvaluateVictory), 0.2f);
    }

    private void EvaluateVictory()
    {
        
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        int aliveEnemies = 0;
        foreach (Enemy enemy in enemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null && !enemyHealth.IsDead)
            {
                aliveEnemies++;
            }
        }

        if (aliveEnemies == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
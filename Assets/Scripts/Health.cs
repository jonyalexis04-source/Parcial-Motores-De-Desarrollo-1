using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isInvulnerable = false;

    public bool hasHitAnimation = false;          
    public bool hyperArmorWhileAttacking = true;  
    public float hitStunCooldown = 2.5f;         
    private float lastHitAnimTime = -99f;

    private Animator animator;

    public bool IsDead => currentHealth <= 0f;

    public Text healthText;
    public GameObject gameOverPanel;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();

        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || isInvulnerable)
        {
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida actual: {currentHealth}");

        UpdateHealthUI();

        if (IsDead)
        {
            Die();
        }
        else if (hasHitAnimation && animator != null)
        {
            Enemy enemyScript = GetComponent<Enemy>();
            bool isAttacking = (enemyScript != null && enemyScript.attacking);

           
            if (hyperArmorWhileAttacking && isAttacking)
            {
                return;
            }

    
            if (Time.time - lastHitAnimTime < hitStunCooldown)
            {
                return;
            }

            lastHitAnimTime = Time.time;

            if (isAttacking)
            {
                enemyScript.FinalAni();
            }

            animator.SetTrigger("Hit");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Vida: {currentHealth}";
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Enemy enemyScript = GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.enabled = false;

            if (enemyScript.enemyWeapon != null)
            {
                enemyScript.enemyWeapon.DisableDamage();
            }

            GameManager manager = FindFirstObjectByType<GameManager>();
            if (manager != null)
            {
                manager.CheckEnemiesAlive();
            }
        }

        PlayerController playerScript = GetComponent<PlayerController>();
        if (playerScript != null)
        {
            playerScript.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }

        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.detectCollisions = false;
        }
    }
}
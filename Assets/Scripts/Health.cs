using UnityEngine;
using UnityEngine.UI;
using MagicPigGames;

public class Health : MonoBehaviour
{
    public bool isFinalBoss = false;

    public float maxHealth = 100f;
    private float currentHealth;
    public float CurrentHealth => currentHealth;

    public bool isInvulnerable = false;

    public bool hasHitAnimation = false;
    public bool hyperArmorWhileAttacking = true;
    public float hitStunCooldown = 2.5f;
    private float lastHitAnimTime = -99f;

    private Animator animator;

    public bool IsDead => currentHealth <= 0f;

    public ProgressBar healthBar;
    public Text healthText;
    public GameObject gameOverPanel;

    public Slider uiHealthSlider;
    public Image uiHealthFillImage;
    public GameObject healthBarRoot;

    public float maxMana = 100f;
    private float currentMana;
    public float CurrentMana => currentMana;
    public ProgressBar manaBar;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        animator = GetComponentInChildren<Animator>();

        UpdateHealthUI();
        UpdateManaUI();
    }

    public void TakeDamage(float amount, bool applyKnockback = false, Vector3 hitSourcePosition = default)
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
            return;
        }

        PlayerController playerScript = GetComponent<PlayerController>();
        if (playerScript != null)
        {
            if (applyKnockback)
            {
                playerScript.ApplyHitFeedback(true, hitSourcePosition);
                return;
            }
            else if (hasHitAnimation)
            {
                if (Time.time - lastHitAnimTime < hitStunCooldown)
                {
                    return;
                }
                lastHitAnimTime = Time.time;
                playerScript.ApplyHitFeedback(false, hitSourcePosition);
                return;
            }
        }

        if (hasHitAnimation && animator != null)
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

            animator.ResetTrigger("Hit");
            animator.SetTrigger("Hit");
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthUI();
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
            UpdateManaUI();
            return true;
        }
        return false;
    }

    public void RestoreMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0f, maxMana);
        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        if (manaBar != null)
        {
            float manaPercent = currentMana / maxMana;
            Debug.Log($"Actualizando barra de maná:" + manaPercent);
            manaBar.SetProgress(manaPercent);
        }
    }

    private void UpdateHealthUI()
    {
        float ratio = maxHealth > 0f ? (currentHealth / maxHealth) : 0f;

        if (healthBar != null)
        {
            healthBar.SetProgress(ratio);
        }

        if (uiHealthSlider != null)
        {
            uiHealthSlider.value = ratio;
        }

        if (uiHealthFillImage != null)
        {
            uiHealthFillImage.fillAmount = ratio;
        }

        if (healthText != null)
        {
            healthText.text = $"Vida: {currentHealth}";
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }

        if (animator != null)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Attack", false);
            animator.ResetTrigger("Die");
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
                if (isFinalBoss)
                {
                    manager.TriggerVictory();
                }
                else
                {
                    manager.CheckEnemiesAlive();
                }
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

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider[] allColliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in allColliders)
        {
            if (c.isTrigger)
            {
                c.enabled = false;
            }
        }
    }

    public void RespawnHeal()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(true);
        }

        UpdateHealthUI();
        UpdateManaUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        PlayerController playerScript = GetComponent<PlayerController>();
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }

        Collider[] allColliders = GetComponentsInChildren<Collider>();
        foreach (Collider c in allColliders)
        {
            c.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
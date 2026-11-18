using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarManager : MonoBehaviour
{
    public int healthPotions = 0;
    public int manaPotions = 0;
    public bool hasKey = false;
    public bool hasAxeUpgrade = false;

    public TMP_Text slot1Text; 
    public TMP_Text slot2Text; 
    public Image keyIcon;  

    private Health playerHealth;
    private PlayerController playerController;

    private void Start()
    {
        playerHealth = GetComponent<Health>();
        playerController = GetComponent<PlayerController>();
        UpdateHotbarUI();
    }

    private void Update()
    {        
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            UseHealthPotion();
        }

        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            UseManaPotion();
        }
    }

    public void AddLoot(LootPickup.LootType type, float value)
    {
        switch (type)
        {
            case LootPickup.LootType.HealthPotion:
                healthPotions++;
                Debug.Log("¡Poción de vida recogida! Total: " + healthPotions);
                break;
            case LootPickup.LootType.ManaPotion:
                manaPotions++;
                Debug.Log("¡Poción de maná recogida! Total: " + manaPotions);
                break;
            case LootPickup.LootType.Key:
                hasKey = true;
                Debug.Log("¡Llave conseguida!");
                break;
            case LootPickup.LootType.AxeUpgrade:
                hasAxeUpgrade = true;
                Debug.Log("¡Mejora de arma conseguida!");
                break;
        }
        UpdateHotbarUI();
    }

    private void UseHealthPotion()
    {
        if (healthPotions > 0 && playerHealth != null)
        {
            healthPotions--;
            playerHealth.Heal(50f);
            Debug.Log("Poción de vida consumida.");
            UpdateHotbarUI();
        }
        else
        {
            Debug.Log("No quedan pociones de vida.");
        }
    }

    private void UseManaPotion()
    {
        if (manaPotions > 0 && playerController != null)
        {
            manaPotions--;
            playerController.RestoreMana(50f);
            Debug.Log("Poción de maná consumida.");
            UpdateHotbarUI();
        }
        else
        {
            Debug.Log("No quedan pociones de maná.");
        }
    }

    private void UpdateHotbarUI()
    {
        if (slot1Text != null)
        {
            slot1Text.text = healthPotions.ToString();
        }


        if (slot2Text != null)
        {
            slot2Text.text = manaPotions.ToString(); ;
        }

        if (keyIcon != null)
        {
            keyIcon.color = hasKey ? Color.white : new Color(1f, 1f, 1f, 0.25f);
        }
    }
}
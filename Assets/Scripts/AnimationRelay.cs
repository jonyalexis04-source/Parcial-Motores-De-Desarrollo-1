using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void OpenComboWindow() => playerController?.OpenComboWindow();
    public void CheckNextComboStep() => playerController?.CheckNextComboStep();
    public void EnableWeaponDamage() => playerController?.EnableWeaponDamage();
    public void DisableWeaponDamage() => playerController?.DisableWeaponDamage();
    public void ResetCombo() => playerController?.ResetCombo();
}

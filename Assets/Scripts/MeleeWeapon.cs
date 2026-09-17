using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 25f;
    private bool canDealDamage = false;

    public bool isEnemyWeapon = false; 

    public void EnableDamage()
    {
        canDealDamage = true;
    }

    public void DisableDamage()
    {
        canDealDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage)
        {
            return;
        }

        if (!isEnemyWeapon && (other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            return;
        }

        if (isEnemyWeapon && other.CompareTag("Enemy"))
        {
            return;
        }

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);

            canDealDamage = false;
        }


    }

}

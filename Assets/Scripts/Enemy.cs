using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int routine;
    public float chronometer;
    public Animator ani;
    public Quaternion angle;
    public float grade;

    public GameObject target;
    public bool attacking;

    public MeleeWeapon enemyWeapon;

    private void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");

        if (enemyWeapon == null)
        {
            enemyWeapon = GetComponentInChildren<MeleeWeapon>();
        }
    }

    public void EnemyBehavior()
    {
        if (target == null)
        {
            return;
        }

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            ani.SetBool("Walk", false);
            ani.SetBool("Run", false);
            ani.SetBool("Attack", false);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 10)
        {
            ani.SetBool("Run", false);
            chronometer += 1 * Time.deltaTime;
            if (chronometer >= 4)
            {
                routine = Random.Range(0, 2);
                chronometer = 0;
            }

            switch (routine)
            {
                case 0:
                    ani.SetBool("Walk", false);
                    break;

                case 1:
                    grade = Random.Range(0, 360);
                    angle = Quaternion.Euler(0, grade, 0);
                    routine++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                    transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                    ani.SetBool("Walk", true);
                    break;
            }
        }
        else
        {
            if (distance > 2f && !attacking)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                ani.SetBool("Walk", false);

                ani.SetBool("Run", true);
                transform.Translate(Vector3.forward * 3 * Time.deltaTime);

                ani.SetBool("Attack", false);
            }
            else
            {
                if (distance <= 2f && !attacking)
                {
                    var lookPos = target.transform.position - transform.position;
                    lookPos.y = 0;
                    if (lookPos != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(lookPos);
                    }

                    ani.SetBool("Walk", false);
                    ani.SetBool("Run", false);


                    ani.SetBool("Attack", true);
                    attacking = true;

                    if (enemyWeapon != null)
                    {
                        enemyWeapon.EnableDamage();
                    }
                }
            }
        }
    }

    public void FinalAni()
    {
        ani.SetBool("Attack", false);
        attacking = false;

        if (enemyWeapon != null)
        {
            enemyWeapon.DisableDamage();
        }
    }

    private void Update()
    {
        EnemyBehavior();
    }
}

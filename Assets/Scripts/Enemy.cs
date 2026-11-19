using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    public float attackCooldown = 0.4f;
    public float maxAttackDuration = 2.5f;
    private float lastAttackEndTime = -99f;
    private float attackTimer;

    public bool useAnimationEventsForDamage = false;

    public float obstacleCheckDistance = 2.2f;
    public float obstacleAvoidStrength = 1.0f;
    public float turnSpeed = 5f;
    public float avoidTurnSpeed = 10f;
    public LayerMask obstacleMask = ~0;

    private float stuckTimer = 0f;
    private Vector3 lastCheckedPos;
    private int forcedAvoidSide = 0; 
    public int routine;
    public float chronometer;
    public Animator ani;
    public Quaternion angle;
    public float grade;

    public GameObject target;
    public bool attacking;

    public MeleeWeapon enemyWeapon;

    private Rigidbody rb;
    private Vector3 desiredHorizontalVelocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        lastCheckedPos = transform.position;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        GameObject foundPlayer = GameObject.FindWithTag("Player");
        target = foundPlayer != null ? foundPlayer : GameObject.Find("Player");

        if (enemyWeapon == null)
        {
            enemyWeapon = GetComponentInChildren<MeleeWeapon>();
        }
    }

    public void EnemyBehavior()
    {
        desiredHorizontalVelocity = Vector3.zero;

        if (target == null)
        {
            return;
        }

        if (attacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= maxAttackDuration)
            {
                FinalAni();
            }
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

        if (distance > detectionRange)
        {
            stuckTimer = 0f;
            forcedAvoidSide = 0;
            lastCheckedPos = transform.position;

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
                    desiredHorizontalVelocity = transform.forward * walkSpeed;
                    ani.SetBool("Walk", true);
                    break;
            }
        }
        else
        {
            if (distance > attackRange)
            {
                Vector3 toTarget = target.transform.position - transform.position;
                toTarget.y = 0f;

                Vector3 desiredDir = toTarget.sqrMagnitude > 0.001f ? toTarget.normalized : transform.forward;
                bool isAvoiding = false;
                Vector3 steeredDir = GetSteeredMoveDirection(desiredDir, out isAvoiding);

                // Detectar si está trabado contra algo mientras intenta correr
                float movedSinceCheck = Vector3.Distance(
                    new Vector3(transform.position.x, 0f, transform.position.z),
                    new Vector3(lastCheckedPos.x, 0f, lastCheckedPos.z)
                );

                if (movedSinceCheck < 0.05f)
                {
                    stuckTimer += Time.fixedDeltaTime;
                    if (stuckTimer >= 0.45f)
                    {
                        if (forcedAvoidSide == 0)
                        {
                            forcedAvoidSide = Random.value > 0.5f ? 1 : -1;
                        }
                        Vector3 side = forcedAvoidSide == 1 ? transform.right : -transform.right;
                        side.y = 0f;
                        steeredDir = (side + desiredDir * 0.35f).normalized;
                        isAvoiding = true;
                    }
                }
                else
                {
                    stuckTimer = 0f;
                    forcedAvoidSide = 0;
                    lastCheckedPos = transform.position;
                }

                if (steeredDir != Vector3.zero)
                {
                    float currentTurnSpeed = isAvoiding ? avoidTurnSpeed : turnSpeed;
                    Quaternion targetRotation = Quaternion.LookRotation(steeredDir);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentTurnSpeed);
                }

                ani.SetBool("Walk", false);
                ani.SetBool("Run", true);
                desiredHorizontalVelocity = transform.forward * runSpeed;
                ani.SetBool("Attack", false);
            }
            else
            {
                stuckTimer = 0f;
                forcedAvoidSide = 0;
                lastCheckedPos = transform.position;

                if (Time.time - lastAttackEndTime >= attackCooldown)
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
                    attackTimer = 0f;

                    if (!useAnimationEventsForDamage && enemyWeapon != null)
                    {
                        enemyWeapon.EnableDamage();
                    }
                }
            }
        }
    }

    private Vector3 GetSteeredMoveDirection(Vector3 desiredDir, out bool isAvoiding)
    {
        isAvoiding = false;
        desiredDir.y = 0f;
        if (desiredDir.sqrMagnitude < 0.0001f)
        {
            return transform.forward;
        }
        desiredDir.Normalize();

        Vector3 origin = transform.position + Vector3.up * 0.8f;
        Vector3 forward = transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.0001f) forward = desiredDir;
        else forward.Normalize();

        bool hitForward = Physics.Raycast(origin, forward, out RaycastHit hit, obstacleCheckDistance, obstacleMask, QueryTriggerInteraction.Ignore);

        if (hitForward)
        {
            bool hitPlayer = target != null && (
                hit.transform == target.transform ||
                hit.transform.IsChildOf(target.transform) ||
                target.transform.IsChildOf(hit.transform)
            );

            if (!hitPlayer)
            {
                isAvoiding = true;

                Vector3 rightDir = new Vector3(forward.z, 0f, -forward.x).normalized;
                Vector3 leftDir = -rightDir;

                bool rightBlocked = Physics.Raycast(origin, (forward + rightDir * 0.8f).normalized, obstacleCheckDistance * 1.3f, obstacleMask, QueryTriggerInteraction.Ignore);
                bool leftBlocked = Physics.Raycast(origin, (forward + leftDir * 0.8f).normalized, obstacleCheckDistance * 1.3f, obstacleMask, QueryTriggerInteraction.Ignore);

                Vector3 sideChoice;
                if (forcedAvoidSide != 0)
                {
                    sideChoice = forcedAvoidSide == 1 ? rightDir : leftDir;
                }
                else if (!rightBlocked && leftBlocked)
                {
                    sideChoice = rightDir;
                }
                else if (!leftBlocked && rightBlocked)
                {
                    sideChoice = leftDir;
                }
                else
                {
                    sideChoice = rightDir;
                }

                Vector3 blended = desiredDir + sideChoice * obstacleAvoidStrength;
                blended.y = 0f;
                return blended.sqrMagnitude > 0.0001f ? blended.normalized : sideChoice;
            }
        }

        return desiredDir;
    }

    private void FixedUpdate()
    {
        EnemyBehavior();

        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = new Vector3(
                desiredHorizontalVelocity.x,
                rb.linearVelocity.y,
                desiredHorizontalVelocity.z
            );
        }
    }

    private void Update()
    {
    }

    public void ActivarPisoton()
    {
        if (enemyWeapon != null)
        {
            enemyWeapon.EnableDamage();

            Collider col = enemyWeapon.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
                col.enabled = true;
            }
        }
    }

    public void DesactivarPisoton()
    {
        if (enemyWeapon != null)
        {
            enemyWeapon.DisableDamage();
        }
    }

    public void FinalAni()
    {
        ani.SetBool("Attack", false);
        attacking = false;
        attackTimer = 0f;
        lastAttackEndTime = Time.time;

        if (enemyWeapon != null)
        {
            enemyWeapon.DisableDamage();
        }
    }

    public void paso()
    {

    }
}
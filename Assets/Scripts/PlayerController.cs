using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 0.15f;
    public float gravity = -9.81f;
    public float jumpCooldown = 0.8f;
    private float nextJumpTime = 0f;

    public Transform cameraTransform;
    public MeleeWeapon meleeWeapon;

    public GameObject weaponInHand;
    public Transform magicSpawnPoint;
    public GameObject magicPrefab;


    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Animator animator;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;

    private int comboStep = 0;
    private bool canCombo = false;
    private bool inputBuffer = false;


    private bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    private bool isAiming = false;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputActions = new PlayerInputActions();
    }

    private void Start()
    {
        ResetCombo();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();


    private void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        isAiming = inputActions.Player.Aim.IsPressed();

        if (weaponInHand != null)
        {
            weaponInHand.SetActive(!isAiming);
        }

        if (animator != null)
        {
            animator.SetBool("IsAiming", isAiming);
        }

        HandleMovement();

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isPlayingJump = stateInfo.IsName("Jump");

        if (isGrounded && !isPlayingJump && inputActions.Player.Jump.triggered && Time.time >= nextJumpTime)
        {
            if (isAttacking)
            {
                ResetCombo();
            }

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            nextJumpTime = Time.time + jumpCooldown;
            
            if (animator != null)
            {
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
            }

        }

        if (inputActions.Player.Attack.triggered)
        {
            OnAttackInput();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void HandleMovement()
    {

        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (isAttacking)
        {
            if (animator != null)
            {
                animator.SetFloat("MoveX", 0f);
                animator.SetFloat("MoveY", 0f);
            }

            return;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        bool isRunning = inputActions.Player.Sprint.IsPressed();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        float speedMultiplier = isRunning ? 1f : 0.5f;

        if (animator != null)
        {
            animator.SetFloat("MoveX", moveInput.x * speedMultiplier, 0.1f, Time.deltaTime);
            animator.SetFloat("MoveY", moveInput.y * speedMultiplier, 0.1f, Time.deltaTime);
        }

        if (moveInput.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        }


    }

    private void OnAttackInput()
    {
        if (isAiming)
        {
            ShootMagic();
        }
        else
        {
            Attack();
        }
    }

    private void ShootMagic()
    {
        
        if (magicPrefab != null && magicSpawnPoint != null)
        {
            Vector3 spawnDirection = cameraTransform.forward;
            Quaternion rotation = Quaternion.LookRotation(cameraTransform.forward);

            GameObject spell = Instantiate(magicPrefab, magicSpawnPoint.position, rotation);
            if (animator != null)
            {
                animator.SetTrigger("CastMagic");
            }
        }
    }

    private void Attack()
    {
        AnimatorStateInfo stateInfo = animator != null ? animator.GetCurrentAnimatorStateInfo(0) : default;
        bool isPlayingJump = animator != null && stateInfo.IsName("Jump");

        if (!isGrounded || isPlayingJump)
        {
            return;
        }

        if (comboStep > 0)
        {
            if (canCombo)
            {
                inputBuffer = true;
            }

            return;
        }

        StartComboStep(1);
    }

    private void StartComboStep(int step)
    {
        comboStep = step;
        canCombo = false;
        inputBuffer = false;
        isAttacking = true;

        if (animator != null)
        {
            animator.ResetTrigger($"AttackRegular{step}");
            animator.SetTrigger($"AttackRegular{step}");
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    public void OpenComboWindow()
    {
        canCombo = true;
    }

    public void CheckNextComboStep()
    {
        if (inputBuffer && comboStep < 3)
        {
            StartComboStep(comboStep + 1);
        }
    }


    public void ResetCombo()
    {
        comboStep = 0;
        canCombo = false;
        inputBuffer = false;
        isAttacking = false;

        if (meleeWeapon != null)
        {
            meleeWeapon.DisableDamage();
        }
    }

    public void EnableWeaponDamage()
    {
        if (meleeWeapon != null)
        {
            meleeWeapon.EnableDamage();
        }
    }

    public void DisableWeaponDamage()
    {
        if (meleeWeapon != null)
        {
            meleeWeapon.DisableDamage();
        }
    }

}
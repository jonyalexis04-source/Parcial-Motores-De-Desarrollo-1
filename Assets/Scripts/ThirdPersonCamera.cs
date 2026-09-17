using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 5.0f;
    public float sensitivity = 0.01f;
    public Vector2 pitchLimits = new Vector2(-20f, 60f);


    public float yaw = 0.0f;
    private float pitch = 5.0f;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void LateUpdate()
    {
        if (!target)
        {
            return;
        }


        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = target.position + offset + Vector3.up * 1.0f;
        transform.LookAt(target.position + Vector3.up * 1.0f);
    }
}

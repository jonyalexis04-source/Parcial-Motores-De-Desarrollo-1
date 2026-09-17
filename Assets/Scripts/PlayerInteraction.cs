using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public Transform interactionPoint;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Vector3 origin = interactionPoint != null ? interactionPoint.position : transform.position;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * interactRange, Color.red, 1f);
        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactRange))
        {
            InteractiveDoor door = hit.collider.GetComponent<InteractiveDoor>();
            if (door != null)
            {
                door.Interact();
            }
        }
    }
}
using UnityEngine;

public class InteractiveDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0f, 4f, 0f); 
    public float speed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    private void Update()
    {
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }
}
using UnityEngine;

public class InteractiveDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0f, 4f, 0f);
    public float speed = 3f;

    public bool requieresKey = true;

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

    public void Interact(GameObject player)
    {
        HotbarManager hotbar = player.GetComponent<HotbarManager>();
        if (requieresKey)
        {
            if (hotbar != null && hotbar.hasKey)
            {
                isOpen = !isOpen;
                Debug.Log("¡Puerta abierta con éxito!");
                hotbar.hasKey = false;
                hotbar.UpdateHotbarUI();
            }
            else
            {
                Debug.Log("La puerta está cerrada con llave. ¡Necesitas encontrarla!");
            }
        }
        else
        {
            isOpen = !isOpen;
        }
    }
}
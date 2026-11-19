using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isReached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isReached && other.CompareTag("Player"))
        {
            if (CheckpointManager.Instance != null)
            {
                isReached = true;
                CheckpointManager.Instance.UpdateCheckpoint(transform.position);
                Debug.Log("¡Checkpoint activado correctamente!");
            }
            else
            {
                Debug.LogWarning("¡Falta el CheckpointManager en la escena!");
            }
        }
    }
}
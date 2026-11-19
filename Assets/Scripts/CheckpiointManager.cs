using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCheckpoint(Vector3 newPosition)
    {
        lastCheckpointPosition = newPosition;
        hasCheckpoint = true;
    }

    public void RespawnPlayer(GameObject player)
    {
        if (!hasCheckpoint)
        {
            return;
        }

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        player.transform.position = lastCheckpointPosition;

        if (controller != null)
        {
            controller.enabled = true;
        }

        Health health = player.GetComponent<Health>();
        if (health != null)
        {
            health.RespawnHeal();
        }
    }
}
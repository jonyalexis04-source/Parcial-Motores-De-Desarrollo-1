using UnityEngine;

public class SonidosPasos : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip golpeclip;

    public void Sonidogolpe()
    {
        if (audioSource == null || golpeclip == null)
        {
            return;
        }

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(golpeclip);
    }

    public void golpe()
    {
        Sonidogolpe();
    }

    public void muerte()
    {
    }
}

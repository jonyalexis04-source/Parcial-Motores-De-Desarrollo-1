using UnityEngine;
using UnityEngine.Audio;

public class DynamicAudioController : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource targetSource;
    public string musicaVolumeParam = "MusicaVol";
    [Range(0f, 1f)] public float lowHealthThreshold = 0.35f;

    public void ActualizarAudioPorEstado(float vidaActual, float vidaMaxima, bool accionActiva)
    {
        if (mixer == null || vidaMaxima <= 0f) return;

        float ratioVida = vidaActual / vidaMaxima;
        bool pocaVida = ratioVida <= lowHealthThreshold;

        float targetDb = (pocaVida || accionActiva) ? -10f : 0f;
        mixer.SetFloat(musicaVolumeParam, targetDb);

        if (targetSource != null)
        {
            targetSource.pitch = pocaVida ? 1.08f : 1.0f;
        }
    }
}
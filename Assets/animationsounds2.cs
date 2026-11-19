using UnityEngine;

public class SonidosPasos : MonoBehaviour
{
    public AudioSource fuenteDeAudio;
    public AudioClip[] sonidosDePasos;


    public float tiempoMinimoEntrePasos = 0.25f;

    private float tiempoUltimoPaso = 0f;

    public void ReproducirPaso(AnimationEvent evt)
    {

        if (evt.animatorClipInfo.weight < 0.35f) return;


        if (Time.time - tiempoUltimoPaso < tiempoMinimoEntrePasos) return;


        if (sonidosDePasos.Length > 0)
        {
            tiempoUltimoPaso = Time.time;

            int indiceAleatorio = Random.Range(0, sonidosDePasos.Length);
            fuenteDeAudio.pitch = Random.Range(0.9f, 1.1f);
            fuenteDeAudio.PlayOneShot(sonidosDePasos[indiceAleatorio]);
        }
    }
}
using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pasoClip;
    public AudioClip ataqueClip;
    public AudioClip dashClip;
    public AudioClip consumibleclip;
    public AudioClip muerteclip;
    public AudioClip golpeclip;
    public AudioClip magiaclip;
    public AudioClip saltoclip;

    [Range(0f, 1f)] public float volumenPaso = 0.4f;
    [Range(0f, 1f)] public float volumenAtaque = 1f;
    [Range(0f, 1f)] public float volumenDash = 1f;

    public float intervaloMinimoPaso = 0.15f;
    float ultimoPaso;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }

    private void ReproducirClip(AudioClip clip, float volumen = 1f)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clip, volumen);
    }

    public void SonidoPaso()
    {
        if (Time.time - ultimoPaso < intervaloMinimoPaso) return;
        ultimoPaso = Time.time;

        ReproducirClip(pasoClip, volumenPaso);
    }

    public void SonidoAtaque()
    {
        ReproducirClip(ataqueClip, volumenAtaque);
    }

    public void SonidoDash()
    {
        ReproducirClip(dashClip, volumenDash);
    }

    public void Sonidoconsumible()
    {
        ReproducirClip(consumibleclip);
    }

    public void Sonidomuerte()
    {
        ReproducirClip(muerteclip);
    }

    public void Sonidogolpe()
    {
        ReproducirClip(golpeclip);
    }

    public void Sonidomagia()
    {
        ReproducirClip(magiaclip);
    }

    public void Sonidosalto()
    {
        ReproducirClip(saltoclip);
    }

    public void golpe() => Sonidogolpe();
    public void ataque() => SonidoAtaque();
    public void paso() => SonidoPaso();
    public void dash() => SonidoDash();
    public void consumible() => Sonidoconsumible();
    public void muerte() => Sonidomuerte();
    public void magia() => Sonidomagia();
    public void salto() => Sonidosalto();
}
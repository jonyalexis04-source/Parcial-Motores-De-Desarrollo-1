using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pasoClip;
    public AudioClip ataqueClip;
    public AudioClip dashClip;
    public AudioClip consumibleclip;

    public float intervaloMinimoPaso = 0.15f;
    float ultimoPaso;

    public void SonidoPaso()
    {
        if (Time.time - ultimoPaso < intervaloMinimoPaso) return;
        ultimoPaso = Time.time;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(pasoClip);
    }

    public void SonidoAtaque()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(ataqueClip);
    }

    public void SonidoDash()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(dashClip);
    }

    public void Sonidoconsumible()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(consumibleclip);
    }


    public void golpe()
    {
        SonidoAtaque();
    }

    public void paso()
    {
        SonidoPaso();
    }

    public void dash()
    {
        SonidoDash();
    }

    public void consumible()
    {
        Sonidoconsumible();
    }
}
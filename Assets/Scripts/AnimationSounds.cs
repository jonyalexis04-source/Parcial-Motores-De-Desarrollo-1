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
    public void Sonidomuerte()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(muerteclip);
    }
    public void Sonidogolpe()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(golpeclip);
    }
    public void Sonidomagia()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(magiaclip);
    }
    public void Sonidosalto()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(saltoclip);
    }

    public void golpe()
    {
        Sonidogolpe();
    }

    public void ataque()
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
    public void muerte()
    {
        Sonidomuerte();
    }
    public void magia()
    {
        Sonidomagia();
    }
    public void salto()
    {
        Sonidosalto();
    }
}
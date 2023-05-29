using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class AudioController
{
    static AudioSource source;
    static AudioClip deathAudio;
    static AudioClip dashAudio;
    static AudioClip healAudio;
    static AudioClip hurtAudio;

    public static void Init()
    {
        source = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        deathAudio = Resources.Load<AudioClip>("Audio/death");
        dashAudio = Resources.Load<AudioClip>("Audio/dash");
        healAudio = Resources.Load<AudioClip>("Audio/heal");
        hurtAudio = Resources.Load<AudioClip>("Audio/hurt");
    }

    public static void PlayDeath()
    {
        source.PlayOneShot(deathAudio);
    }
    public static void PlayDash()
    {
        source.PlayOneShot(dashAudio);
    }
    public static void PlayHeal()
    {
        source.PlayOneShot(healAudio);
    }
    public static void PlayHurt()
    {
        source.PlayOneShot(hurtAudio);
    }

}

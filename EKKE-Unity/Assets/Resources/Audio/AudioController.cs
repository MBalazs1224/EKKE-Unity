using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class AudioController
{
    static AudioClip death;
    static AudioClip dash;
    static AudioClip heal;
    static AudioClip hurt;
    static AudioClip pigeonMove;
    static AudioClip pigeonIdle;
    static AudioClip pigeonIdle2;
    static AudioClip pigeonIdle5G;
    static AudioClip pigeonMove5G;
    static AudioClip pigeonSpot5G;


    public static void Init()
    {
        death = Resources.Load<AudioClip>("Audio/death");
        dash = Resources.Load<AudioClip>("Audio/dash");
        heal = Resources.Load<AudioClip>("Audio/heal");
        hurt = Resources.Load<AudioClip>("Audio/hurt");
        pigeonMove = Resources.Load<AudioClip>("Audio/pigeon/pigeon_move");
        pigeonIdle = Resources.Load<AudioClip>("Audio/pigeon/pigeon_idle");
        pigeonIdle2 = Resources.Load<AudioClip>("Audio/pigeon/pigeon_idle2");
        pigeonIdle5G = Resources.Load<AudioClip>("Audio/5gpigeon/5g_idle");
        pigeonMove5G = Resources.Load<AudioClip>("Audio/5gpigeon/5g_move");
        pigeonSpot5G = Resources.Load<AudioClip>("Audio/5gpigeon/5g_spot");
        
    }

    public static void PlayDeath(AudioSource source)
    {
        source.PlayOneShot(death);
    }
    public static void PlayDash(AudioSource source)
    {
        source.PlayOneShot(dash);
    }
    public static void PlayHeal(AudioSource source)
    {
        source.PlayOneShot(heal);
    }
    public static void PlayHurt(AudioSource source)
    {
        source.PlayOneShot(hurt);
    }
    public static void PlayPigeonMove(AudioSource source)
    {
        source.PlayOneShot(pigeonMove);
    }
    public static void PlayPigeonIdle(AudioSource source)
    {
        source.PlayOneShot(pigeonIdle);
    }    
    public static void PlayPigeonIdle2(AudioSource source)
    {
        source.PlayOneShot(pigeonIdle2);
    }
    public static void PlayPigeonIdle5G(AudioSource source)
    {
        source.PlayOneShot(pigeonIdle5G);
    }
    public static void PlayPigeonMove5G(AudioSource source)
    {
        source.PlayOneShot(pigeonMove5G);
    }
    public static void PlayPigeonSpot5G(AudioSource source)
    {
        source.PlayOneShot(pigeonSpot5G);
    }

}

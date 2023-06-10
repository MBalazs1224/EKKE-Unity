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

    static AudioClip droneDeath;
    static AudioClip droneHurt;
    static AudioClip dronIdle;
    static AudioClip droneSpotAttack;

    static AudioClip policeAttack;
    static AudioClip policeSpot;
    static AudioClip policeTurn;

    static AudioClip rcAttack;
    static AudioClip rcIdle;

    static AudioClip secAttack;
    static AudioClip secDeath;
    static AudioClip secHurt;
    static AudioClip secMove;
    static AudioClip secSpot;

    static AudioClip shieldGatya;
    static AudioClip shieldMove;
    static AudioClip shieldRecovery;
    static AudioClip shieldSpot;

    static AudioClip notificationIn;
    static AudioClip notificationOut;

    public static float soundWaitTime = 5f;

    static AudioSource source;
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

        droneDeath = Resources.Load<AudioClip>("Audio/dron/dron_death");
        droneHurt = Resources.Load<AudioClip>("Audio/dron/dron_hurt");
        dronIdle = Resources.Load<AudioClip>("Audio/dron/dron_idle");
        droneSpotAttack = Resources.Load<AudioClip>("Audio/dron/dron_spot_attack");
        
        policeAttack = Resources.Load<AudioClip>("Audio/police/police_attack");
        policeSpot = Resources.Load<AudioClip>("Audio/police/police_spot");
        policeTurn = Resources.Load<AudioClip>("Audio/police/police_turn");

        rcAttack = Resources.Load<AudioClip>("Audio/rc/rc_attack");
        rcIdle = Resources.Load<AudioClip>("Audio/rc/rc_idle");


        secAttack = Resources.Load<AudioClip>("Audio/sec/sec_attack");
        secDeath = Resources.Load<AudioClip>("Audio/sec/sec_death");
        secHurt = Resources.Load<AudioClip>("Audio/sec/sec_hurt");
        secMove = Resources.Load<AudioClip>("Audio/sec/sec_move");
        secSpot = Resources.Load<AudioClip>("Audio/sec/sec_spot");

        shieldMove = Resources.Load<AudioClip>("Audio/shield/shield_move");
        shieldRecovery = Resources.Load<AudioClip>("Audio/shield/shield_recovery_1");
        shieldGatya = Resources.Load<AudioClip>("Audio/shield/shield_gatyalehuzas");
        shieldSpot = Resources.Load<AudioClip>("Audio/shield/shield_spot_1");


        notificationIn = Resources.Load<AudioClip>("Audio/notification_in");
        notificationOut = Resources.Load<AudioClip>("Audio/notification_out");

        source = GameObject.Find("Player").GetComponent<AudioSource>();
        
    }

    public static void PlayNotificationIn()
    {
        source.PlayOneShot(notificationIn);
    }
    public static void PlayNotificationOut()
    {
        source.PlayOneShot(notificationOut);
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
    public static void PlayDroneIdle(AudioSource source)
    {
        source.PlayOneShot(dronIdle);
    }
    public static void PlayDroneHurt(AudioSource source)
    {
        source.PlayOneShot(droneHurt);
    }
    public static void PlayDroneDeath(AudioSource source)
    {
        source.PlayOneShot(droneDeath);
    }
    public static void PlayDroneSpotAttack(AudioSource source)
    {
        source.PlayOneShot(droneSpotAttack);
    }
    public static void PlayPoliceAttack(AudioSource source)
    {
        source.PlayOneShot(policeAttack);
    }
    public static void PlayPoliceSpot(AudioSource source)
    {
        source.PlayOneShot(policeSpot);
    }
    public static void PlayPoliceTurn(AudioSource source)
    {
        source.PlayOneShot(policeTurn);
    }
    public static void PlayRcAttack(AudioSource source)
    {
        source.PlayOneShot(rcAttack);
    }
    public static void PlayRcIdle(AudioSource source)
    {
        source.PlayOneShot(rcIdle);
    }
    public static void PlaySecAttack(AudioSource source)
    {
        source.PlayOneShot(secAttack);
    }
    public static void PlaySecDeath(AudioSource source)
    {
        source.PlayOneShot(secDeath);
    }
    public static void PlaySecHurt(AudioSource source)
    {
        source.PlayOneShot(secHurt);
    }
    public static void PlaySecMove(AudioSource source)
    {
        source.PlayOneShot(secMove);
    }
    public static void PlaySecSpot(AudioSource source)
    {
        source.PlayOneShot(secSpot);
    }

    public static void PlayShieldMove(AudioSource source)
    {
        source.PlayOneShot(shieldMove);
    }
    public static void PlayShieldGatya(AudioSource source)
    {
        source.PlayOneShot(shieldGatya);
    }
    public static void PlayShieldRecovery(AudioSource source)
    {
        source.PlayOneShot(shieldRecovery);
    }
    public static void PlayShieldSpot(AudioSource source)
    {
        source.PlayOneShot(shieldSpot);
    }

}

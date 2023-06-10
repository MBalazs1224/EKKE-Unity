using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class UIController
{
    public class MB : MonoBehaviour { }


    static SpriteRenderer dashRenderer;
    static SpriteRenderer hearthRenderer;
    static SpriteRenderer pauseRenderer;

    static Sprite dash;
    static Sprite cantDash;
    static Sprite hearth;
    static Sprite hearthHurt;
    static Sprite hearthDeath;
    static Sprite paused;

    static Animator hearthAnim;
    static Animator pauseAnim;

    static SceneController sc;

    static GameObject gameObject;

    static MB mb;

    static float notificationHiddenPostition;
    static float notificationShownPostition;

    static Sprite savedNotification;
    static Sprite powerNotification;

    static SpriteRenderer notificationRenderer;
    static GameObject notificationObject;

    [SerializeField]
    private static float notificationMoveSpeed = -40f;
    private static float notificationWaitTime = 1f;

    public static void Init()
    {
        dashRenderer = GameObject.Find("Dash").GetComponent<SpriteRenderer>();
        hearthRenderer = GameObject.Find("Hearth").GetComponent<SpriteRenderer>();

        dash = Resources.Load<Sprite>("HUD/dash_active");
        cantDash = Resources.Load<Sprite>("HUD/dash_inactive");
        hearth = Resources.Load<Sprite>("HUD/hearth");
        hearthHurt = Resources.Load<Sprite>("HUD/hearth_hurt");
        hearthDeath = Resources.Load<Sprite>("HUD/hearth_death");

        hearthAnim = GameObject.Find("Hearth").GetComponent<Animator>();
        sc = GameObject.Find("SceneController").GetComponent<SceneController>();
        gameObject = GameObject.Find("UI");

        pauseAnim = GameObject.Find("Pause").GetComponent<Animator>();
        pauseRenderer = GameObject.Find("Pause").GetComponent<SpriteRenderer>();
        paused = Resources.Load<Sprite>("pause");
        pauseAnim.enabled = false;
        pauseRenderer.sprite = null;

        savedNotification = Resources.Load<Sprite>("HUD/gamesaved");
        powerNotification = Resources.Load<Sprite>("HUD/regeneration");
        notificationObject = GameObject.Find("UI Notification");
        notificationRenderer = notificationObject.GetComponent<SpriteRenderer>();

        GameObject go = new GameObject("rakos");
        mb = go.AddComponent<MB>();
    }

    public static void CanDash()
    {
        dashRenderer.sprite = dash;
    }
    public static void CantDash()
    {
        dashRenderer.sprite = cantDash;
    }

    public static void Update()
    {
        notificationShownPostition = Camera.main.transform.position.x + 45;
        notificationHiddenPostition = Camera.main.transform.position.x + 60;
    }


    static IEnumerator ActivateNotification(bool isSave)
    {
        Debug.Log("Save notification started moving");
        notificationRenderer.sprite = isSave ? savedNotification : powerNotification;
        AudioController.PlayNotificationIn();
        while (notificationObject.transform.position.x >= notificationShownPostition)
        {
            notificationObject.transform.position += new Vector3(notificationMoveSpeed, 0) * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(notificationWaitTime);
        sc.StartCoroutine(HideNotification());
    }

    static IEnumerator HideNotification()
    {
        AudioController.PlayNotificationOut();

        while (notificationObject.transform.position.x <= notificationHiddenPostition)
        {
            notificationObject.transform.position += new Vector3(-notificationMoveSpeed, 0) * Time.deltaTime;
            yield return null;
        }
    }

    public static void ActivateSaveNotification()
    {
        sc.StartCoroutine(ActivateNotification(true));
    }
    public static void ActivatePowerNotification()
    {
        sc.StartCoroutine(ActivateNotification(false));
    }



    public static void TakeDamage()
    {
        hearthAnim.enabled = true;
        hearthAnim.SetTrigger("Hurt");
        mb.StartCoroutine(TakeDamageEnumerator());
    }

    static IEnumerator TakeDamageEnumerator()
    {
        yield return null;
        yield return new WaitForSeconds(hearthAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        hearthAnim.enabled = false;
        hearthRenderer.sprite = hearthHurt;

    }

    public static void Death()
    {
        hearthAnim.enabled = true;
        hearthAnim.SetTrigger("Death");
        mb.StartCoroutine(DeathEnumerator());
    }
    static IEnumerator DeathEnumerator()
    {
        yield return null;
        yield return new WaitForSeconds(hearthAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        hearthAnim.enabled = false;
        hearthRenderer.sprite = hearthDeath;

    }
    public static void Heal()
    {
        hearthAnim.enabled = true;
        hearthAnim.SetTrigger("Heal");
        mb.StartCoroutine(HealEnumerator());
    }
    static IEnumerator HealEnumerator()
    {
        yield return null;
        yield return new WaitForSeconds(hearthAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        hearthAnim.enabled = false;
        hearthRenderer.sprite = hearth;

    }
    public static void Pause()
    {
        pauseAnim.enabled = true;
        gameObject.SetActive(false);
        pauseAnim.SetTrigger("Open");
        mb.StartCoroutine(PauseEnumerator());

    }

    static IEnumerator PauseEnumerator()
    {
        yield return null;
        yield return new WaitForSeconds(pauseAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        pauseAnim.enabled = false;
        pauseRenderer.sprite = paused;
        Time.timeScale = 0;

    }
    public static void Unpause()
    {
        //pauseAnim.enabled = true;
        //pauseAnim.SetTrigger("Close");
        mb.StartCoroutine(Enum2());
    }

    //static IEnumerator UnpauseEnumerator()
    //{


    //}


    static IEnumerator Enum2()
    {
        yield return new WaitForSeconds(0.5f);
        pauseAnim.enabled = false;
        gameObject.SetActive(true);
        pauseRenderer.sprite = null;
        Time.timeScale = 1;
        Debug.Log("Csicska");
    }

}

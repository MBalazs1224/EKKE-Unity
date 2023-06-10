using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class CreditsVideo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    VideoPlayer backgroundVideo;
    void Start()
    {
        InputMaster im = new InputMaster();
        im.Enable();
        im.Player.Attack.started += (_) =>
        {
            EndVideo();
        };

        backgroundVideo = GameObject.Find("Background video").GetComponent<VideoPlayer>();

        VideoPlayer vp = this.gameObject.GetComponent<VideoPlayer>();
        vp.loopPointReached += (_) =>
        {
            EndVideo();
        };

        vp.started += (_) =>
        {
            backgroundVideo.SetDirectAudioMute(0, true);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndVideo();
        }
    }

    void EndVideo()
    {
        this.gameObject.SetActive(false);
        mainMenu.SetActive(true);
        backgroundVideo.SetDirectAudioMute(0, false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject mainMenu;
    void Start()
    {
        VideoPlayer vp = this.gameObject.GetComponent<VideoPlayer>();
        vp.loopPointReached += (vp) =>
        {
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        };
    }

}

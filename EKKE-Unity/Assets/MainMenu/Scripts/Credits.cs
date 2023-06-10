using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject vpObject;


    public void StartCredits()
    {
        vpObject.SetActive(true);
        mainMenu.SetActive(false);

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static GameObject main;

    public static GameObject Main
    {
        get { return main; }
    }


    public void Start()
    {
        main = GameObject.Find("Player");
    }
}

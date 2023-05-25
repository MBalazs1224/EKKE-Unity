using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    [SerializeField]
    public Transform player;
    private float lastPlayerX;

    void Start()
    {
        lastPlayerX = player.position.x;
    }

    void Update()
    {
        float deltaX = player.position.x - lastPlayerX;
        transform.position += new Vector3(deltaX * scrollSpeed, 0, 0);
        lastPlayerX = player.position.x;
    }
}

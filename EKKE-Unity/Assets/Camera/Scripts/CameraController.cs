using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    Transform target;
    Vector3 velocity = Vector3.zero;
    [SerializeField]float dampTime = 0.5f;
    [SerializeField]
    float camHeight = 5;


    public float shakeIntesity = 1;
    public float shakeDuration = 0.5f;
    private float shakeTimer = 0f;
    private Transform cameraTransform;



    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
    }

    public void Shake()
    {
        shakeTimer = shakeDuration;
        StartCoroutine(ShakeCoroutine());
    }

    public IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(transform.localPosition.x - shakeIntesity, transform.localPosition.x + shakeIntesity);
            float y = UnityEngine.Random.Range(transform.localPosition.y - shakeIntesity, transform.localPosition.y + shakeIntesity);

            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    void Start()
    {
        cam = Camera.main;
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position + new Vector3 (0,camHeight) - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }   
    }
}

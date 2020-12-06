using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject mainCamera;
    public float parallaxEffect;

    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cameraTransform = mainCamera.GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        Vector3 cameraPos = cameraTransform.position;
        float temp = (cameraPos.x * (1 - parallaxEffect));
        float dist = (cameraPos.x * parallaxEffect);
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(startPos + dist, currentPos.y, currentPos.z);
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}

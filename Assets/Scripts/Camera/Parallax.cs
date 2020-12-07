using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPosX, startPosY;
    public GameObject mainCamera;
    public float parallaxEffect;

    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cameraTransform = mainCamera.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 cameraPos = cameraTransform.position;
        float tempX = (cameraPos.x * (1 - parallaxEffect));
        float distX = (cameraPos.x * parallaxEffect);
        //float tempY = (cameraPos.y * (1 - parallaxEffect));
        float distY = (cameraPos.y * parallaxEffect);
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(startPosX + distX, startPosY + distY, currentPos.z);
        if (tempX > startPosX + length)
        {
            startPosX += length;
        }
        else if (tempX < startPosX - length)
        {
            startPosX -= length;
        }
    }
}

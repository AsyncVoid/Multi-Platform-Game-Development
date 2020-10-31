using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 3, -5);
    public GameObject player;

    private Transform cameraTransform;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = this.GetComponent<Transform>();
        playerTransform = player.GetComponent<Transform>();
        cameraTransform.position = playerTransform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        cameraTransform.position = playerTransform.position + offset;
    }
}

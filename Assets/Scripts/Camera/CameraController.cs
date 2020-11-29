using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Transform cameraTransform;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = this.GetComponent<Transform>();
        playerTransform = player.GetComponent<Transform>();
        cameraTransform.position = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Camera tracks player;
    private void LateUpdate()
    {
        cameraTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 3, playerTransform.position.z - 5f);
    }
}

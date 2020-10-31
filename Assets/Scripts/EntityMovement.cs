using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private float Speed;
    private Vector3 ItemVelocity;
    private EnvironmentController environmentController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject environment = GameObject.FindGameObjectWithTag("EnvironmentController");
        environmentController = environment.GetComponent<EnvironmentController>();
        Speed = environmentController.GetMovementSpeed();
        Debug.Log("Speed: " + Speed);
        ItemVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    private void Update()
    {
        //Speed = environmentController.GetMovementSpeed();
        //ItemVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    void FixedUpdate()
    {
        transform.position += ItemVelocity * Time.deltaTime;
    }

    public void HaltTileMovement()
    {
        ItemVelocity = new Vector3(0, 0, 0);
    }

    public void RestoreTileMovement()
    {
        ItemVelocity = new Vector3(-1 * Speed, 0, 0);
    }
}

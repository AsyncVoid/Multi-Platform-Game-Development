﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{

    private GameObject player;
    private EnvironmentController environmentController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        environmentController = Object.FindObjectOfType<EnvironmentController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = transform.position - player.transform.position;
        //Debug.Log("Distance: " + distance.magnitude);
        if (distance.magnitude < 5)
        {
            environmentController.TriggerCombatScene();
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public Vector3 SpawnPosition;
    public float StandardTime;
    private float timePassed;

    public GameObject RedGem;
    public GameObject BattleTurtle;

    private EnvironmentController EnvironmentController;

    // Start is called before the first frame update
    void Start()
    {
        EnvironmentController = GameObject.FindWithTag("EnvironmentController").GetComponent<EnvironmentController>();
    }

    private void FixedUpdate()
    {
        if (!EnvironmentController.GetIsCombatScene())
        {

            timePassed += Time.deltaTime;
            if (timePassed > StandardTime)
            {
                timePassed = 0f;
                float value = Random.Range(0f, 10f);
                if (value < 2f)
                {
                    Instantiate(BattleTurtle, SpawnPosition, Quaternion.identity);
                }
                else if (value < 5f)
                {
                    Instantiate(RedGem, SpawnPosition, Quaternion.identity);
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

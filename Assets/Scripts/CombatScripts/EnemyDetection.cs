using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{

    private GameObject player;
    private Transform target;
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

        if (!environmentController.GetComponent<EnvironmentController>().GetIsCombatScene() && gameObject.tag == "Enemy")
        {
            if (distance.magnitude < 15)
            {
                environmentController.TriggerCombatScene(gameObject);
            }
        }
    }
}

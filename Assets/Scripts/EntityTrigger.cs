using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTrigger : MonoBehaviour
{

    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Player.GetComponent<PlayerController>().ReturnState())
        {
            Destroy(gameObject);
        }
    }

}

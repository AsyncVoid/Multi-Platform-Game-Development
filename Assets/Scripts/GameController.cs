using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private GameObject player;
    public GameObject Entity;

    public Text playerStateText;
    public Text playerCoolDownText;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(SpawnEntities());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.GetComponent<PlayerController>().ReturnState())
        {
            playerStateText.text = "State: Liquid";
        }
        else {
            playerStateText.text = "State: Normal";
        }

        if (player.GetComponent<PlayerController>().ReturnCooldown())
        {
            playerCoolDownText.text = "Cooldown Inactive";
        }
        else {
            playerCoolDownText.text = "Cooldown Active";
        }
    }

    IEnumerator SpawnEntities() {
        System.Random rand = new System.Random();

        while (true) {
            Instantiate(Entity, new Vector3((float)rand.Next(-50, 50), 1f, 0f), Quaternion.identity);

            yield return new WaitForSeconds(3); 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloorSprite : MonoBehaviour
{

    public GameObject FloorTile;
    private EnvironmentController EnvironmentController;


    // Start is called before the first frame update
    void Start()
    {
        EnvironmentController = GameObject.FindWithTag("EnvironmentController").GetComponent<EnvironmentController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Floor")
        {
            GameObject newFloor = Instantiate(FloorTile, new Vector3(14.4f, -1.5f, 0f), Quaternion.identity);
            newFloor.GetComponent<TileMovement>().SetSpeed(EnvironmentController.GetMovementSpeed());
        }
    }

}

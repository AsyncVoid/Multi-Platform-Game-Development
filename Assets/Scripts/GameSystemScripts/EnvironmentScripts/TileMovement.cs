using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{

    private float Speed;
    private Vector3 TileVelocity;

    // Start is called before the first frame update
    void Start()
    {
        TileVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    // Moves the sprite.
    void FixedUpdate()
    {
        transform.position += TileVelocity * Time.deltaTime;
    }

    public void SetSpeed(float environmentSpeed) {
        Speed = environmentSpeed;
        TileVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    public void HaltTileMovement() {
        TileVelocity = new Vector3(0, 0, 0);
    }

    public void RestoreTileMovement() {
        TileVelocity = new Vector3(-1 * Speed, 0, 0);
    }
}

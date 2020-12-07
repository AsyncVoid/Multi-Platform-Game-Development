using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockXAxis : MonoBehaviour
{
    private float x;

    // Start is called before the first frame update
    void Start()
    {
        this.x = transform.position.x;
    }

    // Update is called once per frame. Locks x Axis of player.
    void LateUpdate()
    { 
        Vector3 before = transform.position;
        transform.position = new Vector3(x, before.y);
    }
}

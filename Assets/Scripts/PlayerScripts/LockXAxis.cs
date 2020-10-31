using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockXAxis : MonoBehaviour
{

    //private Transform transform;
    private float x;
    // Start is called before the first frame update
    void Start()
    {
        //this.transform = this.GetComponent<Transform>();
        this.x = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        Vector3 before = transform.position;
        transform.position = new Vector3(x, before.y);
    }
}

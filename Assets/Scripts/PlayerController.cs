using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    //private Rigidbody2D rb2d; // For 2D models
    private Rigidbody rb;

    private bool Liquified;
    private bool LiquifiedOffCooldown;

    private int LiquifiedLength;
    private int LiquifiedCooldown;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
        //rb2d = GetComponent<Rigidbody2D> ();        
        Liquified = false;
        LiquifiedLength = 3;
        LiquifiedCooldown = 3;
        LiquifiedOffCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) 
        {
            if (LiquifiedOffCooldown)
            {
                StartCoroutine(TimedStateChange()); 
            }
        }
    }

    void FixedUpdate() 
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3 (moveHorizontal, moveVertical);
        //rb2d.AddForce(movement*speed);
        rb.AddForce(movement*speed);

    }

    private void ChangeLiquidState() 
    {
        Liquified ^= true;
    }

    private void UpdateLiquifiedCooldown() 
    {
        LiquifiedOffCooldown ^= true;
    }

    IEnumerator TimedStateChange() 
    {
        ChangeLiquidState();
        UpdateLiquifiedCooldown();
        yield return new WaitForSeconds(LiquifiedLength);
        ChangeLiquidState();
        yield return new WaitForSeconds(LiquifiedCooldown);
        UpdateLiquifiedCooldown();
    }

    public bool ReturnState() {
        return Liquified;
    }

    public bool ReturnCooldown() {
        return LiquifiedOffCooldown;
    }

}



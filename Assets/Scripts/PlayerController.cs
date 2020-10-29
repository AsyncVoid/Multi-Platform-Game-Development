using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float Speed;
    private Rigidbody Rb;

    private bool Liquified;
    private bool LiquifiedOffCooldown;

    private int LiquifiedLength;
    private int LiquifiedCooldown;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Speed = 5;

        Liquified = false;
        LiquifiedLength = 1;
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
        float translation = Input.GetAxis("Horizontal") * Speed;

        if (translation != 0){
            transform.Translate(translation * Time.deltaTime, 0, 0);
        }
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



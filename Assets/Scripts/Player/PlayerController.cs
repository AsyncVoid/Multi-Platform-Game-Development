using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    private float Speed;
    private Rigidbody2D rb;

    private bool Liquified;
    private bool LiquifiedOffCooldown;

    private int LiquifiedLength;
    private int LiquifiedCooldown;

    public Vector3 jump = new Vector2(0.0f, 1.0f);
    public float jumpForce = 2.0f;
    public bool isGrounded;
    private Material material;
    public float liquidTransparency = 0.2f;

    private Dictionary<Skill, int> skillLevels = new Dictionary<Skill, int>();

    // Start is called before the first frame update
    void Start()
    {
        Liquified = false;
        LiquifiedLength = 1;
        LiquifiedCooldown = 3;
        LiquifiedOffCooldown = true;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for keyboard inputs and assign the correct player movements and state changes.
        if (Input.GetKeyDown("space"))
        {
            if (LiquifiedOffCooldown)
            {
                StartCoroutine(TimedStateChange());
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    // Switch between Liquidfied and Non Liquified state.
    private void ChangeLiquidState()
    {
        Liquified ^= true;
        Color color = material.color;
        if (Liquified)
            color.a = liquidTransparency;
        else
            color.a = 1.0f;
        material.color = color;
    }

    // Activate / Deactivate Engulf cooldowns.
    private void UpdateLiquifiedCooldown()
    {
        LiquifiedOffCooldown ^= true;
    }

    // Applies state changes and cooldowns.
    IEnumerator TimedStateChange()
    {
        ChangeLiquidState();
        UpdateLiquifiedCooldown();
        yield return new WaitForSeconds(LiquifiedLength);
        ChangeLiquidState();
        yield return new WaitForSeconds(LiquifiedCooldown);
        UpdateLiquifiedCooldown();
    }

    public bool ReturnState()
    {
        return Liquified;
    }

    public bool ReturnCooldown()
    {
        return LiquifiedOffCooldown;
    }

    // Check for player on ground.
    public void OnCollisionStay2D()
    {
        isGrounded = true;
    }
}
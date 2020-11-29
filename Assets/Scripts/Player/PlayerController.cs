using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;

    private bool Liquified;
    private bool LiquifiedOffCooldown;

    private int LiquifiedLength;
    private int LiquifiedCooldown;

    private bool AttackOffCooldown;
    private bool AttackState;

    private int AttackCooldown;

    public Vector3 jump = new Vector2(0.0f, 1.0f);
    public float jumpForce = 5.0f;
    public bool isGrounded;

    public bool isFacingRight;
    private bool isMoving;

    private float horizontalMovement = 1.0f;

    private Material material;
    public float liquidTransparency = 0.2f;

    private int IdleTimer;
    private float LastInputTime;

    public SpriteRenderer playerSprite;
    private GameObject PlayerModel;

    private Dictionary<Skill, int> skillLevels = new Dictionary<Skill, int>();

    // Start is called before the first frame update
    void Start()
    {
        Liquified = false;
        LiquifiedLength = 1;
        LiquifiedCooldown = 3;
        LiquifiedOffCooldown = true;

        AttackState = false;
        AttackOffCooldown = true;
        AttackCooldown = 1;

        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;
        material = playerSprite.material;

        PlayerModel = GameObject.FindWithTag("PlayerModel");
    }

    // Update is called once per frame
    void Update()
    {
        //Check for keyboard inputs and assign the correct player movements and state changes.
        if (Input.GetKeyDown("space"))
        {
            if (LiquifiedOffCooldown && !AttackState)
            {
                StartCoroutine(TimedStateChange());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (AttackOffCooldown && !Liquified) {
                StartCoroutine(PrimeAttackState());
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        horizontalMovement = Input.GetAxis("Horizontal");

        if (horizontalMovement > 0)
        {
            StartCoroutine(RightHeld());
            animator.SetBool("leftHeld", false);
        }
        else if (horizontalMovement < 0) 
        {
            animator.SetBool("rightHeld", false);
            StartCoroutine(LeftHeld());
        }
        else
        {
            animator.SetBool("rightHeld", false);
            animator.SetBool("leftHeld", false);
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

    private void ChangeAttackState()
    {
        AttackState ^= true;
        Color color = material.color;

        if (AttackState)
        {
            color.b = 0.5f;
            color.g = 0.5f;
        }
        else
        {
            color.b = 1f;
            color.g = 1f;
        }
        material.color = color;
    }

    private void UpdateAttackCooldown() 
    {
        AttackOffCooldown ^= true;
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

    IEnumerator PrimeAttackState() {
        ChangeAttackState();
        UpdateAttackCooldown();
        yield return new WaitForSeconds(2);
        ChangeAttackState();
        yield return new WaitForSeconds(AttackCooldown);
        UpdateAttackCooldown();
    }


    public bool ReturnState()
    {
        return Liquified;
    }

    public bool GetAttackState() {
        return AttackState;
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

    IEnumerator RightHeld() 
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        animator.SetBool("rightHeld", true);
    }

    IEnumerator LeftHeld() 
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        animator.SetBool("leftHeld", true);
    }
}
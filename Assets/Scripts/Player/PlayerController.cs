﻿using System.Collections;
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

    private bool rightToggle;
    private bool leftToggle;

    private Material material;
    public float liquidTransparency = 0.2f;

    private int IdleTimer;
    private float LastInputTime;

    public SpriteRenderer playerSprite;
    private GameObject PlayerModel;
    public Player player;

    public Skill skill;
    public GameObject skillPrefab;

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
        player = GetComponent<Player>();

        rightToggle = false;
        leftToggle = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha2)) {

            if (player.UseMatter(skill.GetMatterUsage())){

                skillPrefab = skill.GetPrefab();

                GameObject skillUsed = Instantiate(skillPrefab, transform.position, Quaternion.identity);
                Projectile tempName = skillUsed.GetComponent<Projectile>();
                tempName.sourceObject = gameObject;

                Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                Vector3 targetPosition = new Vector3(worldPosition.x, worldPosition.y, 0.0f);

                tempName.projectileDirection = (targetPosition - transform.position).normalized;
                tempName.skill = skill;
            }
            else {
                Debug.Log("Out of matter!");
            }
            
        }

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

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            rightToggle ^= true;
            leftToggle = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            leftToggle ^= true;
            rightToggle = false;
        }

        if (rightToggle)
        {
            StartCoroutine(RightHeld());
            animator.SetBool("leftHeld", false);
        }
        else if (leftToggle) 
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
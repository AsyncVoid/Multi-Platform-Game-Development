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

    private bool rightToggle;
    private bool leftToggle;

    private Material material;
    public float liquidTransparency = 0.2f;

    private int IdleTimer;
    private float LastInputTime;

    public SpriteRenderer playerSprite;
    private GameObject PlayerModel;
    private Player player;

    public Skill skill;
    private MenuController menuController;
    private Dictionary<string, Skill> hm;

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
        
        menuController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();        
    }

    // Update is called once per frame
    void Update()
    {
        hm = menuController.hotkeyDict;
        // Basic Attack
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (AttackOffCooldown && !Liquified)
            {
                StartCoroutine(PrimeAttackState());
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            skill = hm["2"];
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skill = hm["3"];
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skill = hm["4"];
        }

        //Check for mouse click to use skill.

        if (Input.GetMouseButtonDown(0)) {
            if (!(skill is null))
            {
                if (player.UseMatter(skill.GetMatterUsage()))
                {
                    // Get component which contains the interface for using a skill.
                    ISkill skillInterface = skill.GetPrefab().GetComponent<ISkill>();

                    // Get mouse pointer position.
                    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                    Vector3 targetPosition = new Vector3(worldPosition.x, worldPosition.y, 0.0f);
                    Vector3 targetDirection = (targetPosition - transform.position).normalized;

                    // Calls the interface method to trigger using a skill. If there's no target just send a random vector3 into the last parameter.
                    skillInterface.UseSkill(skill, gameObject, targetDirection);
                }
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

        // Player Jumping
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Player Toggle right and left movement.
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            rightToggle ^= true;
            leftToggle = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            leftToggle ^= true;
            rightToggle = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            rightToggle = false;
            leftToggle = false;
        }

        // If right or left movement toggled, it will play the animation which also controls the movement.
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

    // Change attack state and indicators of attacking.
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

    // Updates the attack cooldown.
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

    // Allows the user to trigger an attack in the time frame of attackcooldown.
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
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tilemap")
        {
            isGrounded = true;
        }
    }

    // Ensures the animation plays out fully before triggering same animation.
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
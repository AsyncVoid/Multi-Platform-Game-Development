using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private AttackSensor attackSensor;

    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    private GameObject player;
    private Enemy enemy;
    private EnemyController enemyController;
    private DamageController damageController;
    private float inputX;
    private RaycastHit2D hit;

    private Vector3 oldPos;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();

        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        attackSensor = transform.Find("AttackSensor").GetComponent<AttackSensor>();
        damageController = GameObject.FindWithTag("DamageController").GetComponent<DamageController>();

        player = GameObject.FindWithTag("Player");
        enemy = GetComponent<Enemy>();
        inputX = 0f;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Handle input and movement and decisions
        inputX = 0f;
        if (!m_isDead)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float direction = Vector3.Dot(playerDirection, Vector3.right);

            if (playerDistance < 20f && playerDistance > 15f)
            {
                if (direction > 0.1f)
                {
                    inputX = 0.7f;
                }
                else if (direction < -0.1f)
                {
                    inputX = -0.7f;
                }
            }
            else if (playerDistance < 15f && playerDistance > 12f)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    m_animator.SetBool("MagicAttack", false);
                }
                else
                {
                    m_animator.SetBool("MagicAttack", true);
                }
            }
            else if (playerDistance < 12f && playerDistance > 8f)
            {
                if (direction > 0.1f)
                {
                    inputX = -0.8f;
                }
                else
                {
                    inputX = 0.8f;
                }
            }
            else if (playerDistance < 8f) {
                if (direction > 0.1f)
                {
                    inputX = 0.1f;
                }
                else
                {
                    inputX = -0.1f;
                }

                if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    m_animator.SetBool("Attack", false);
                }
                else
                {
                    m_animator.SetBool("Attack", true);
                }
            }
        }

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_animator.SetFloat("RunSpeed", Mathf.Clamp(System.Math.Abs(inputX) * 1.5f, 0.7f, 1f));
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // Handle Animations

        //Death
        if (enemy.ReturnDeathStatus())
        {
            m_isDead = true;
        }

        if (m_isDead)
        {
            m_animator.SetTrigger("isDead");
        }

        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    void FixedUpdate()
    {
        int layerMask = 1 << 8;

        if (inputX > 0)
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.left, 1.4f, layerMask);
            Jump();
        }
        else if (inputX < 0)
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1f), Vector2.right, 1.4f, layerMask);
            Jump();
        }

    }

    private void Jump()
    {
        if (hit.collider != null)
        {
            if (m_grounded)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x * 0.8f, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
        }
    }

    public void Attack()
    {
        if (attackSensor.AttackConfirm())
        {
            damageController.DamagePlayer(enemy);

            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float direction = Vector3.Dot(playerDirection, Vector3.right);

            player.GetComponent<Rigidbody2D>().AddForce(new Vector3(5f * direction, 0f, 0f), ForceMode2D.Impulse);
        }
    }

    public void MagicAttack() {
        Skill skill = enemy.ReturnSkill();
        ISkill skillInterface = skill.GetPrefab().GetComponent<ISkill>();

        Transform staffPoint = transform.Find("Staff");

        Vector3 targetPosition = player.transform.position;
        Vector3 targetDirection = (targetPosition - staffPoint.position).normalized;

        skillInterface.UseSkill(skill, gameObject, targetDirection);
    }
}

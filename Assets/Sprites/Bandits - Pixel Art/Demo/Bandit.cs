using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    private GameObject player;
    private Enemy enemy;
    private EnemyController enemyController;
    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();

        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();

        player = GameObject.FindWithTag("Player");
        enemy = GetComponent<Enemy>();
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

        // -- Handle input and movement --

        float inputX = 0f;

        if (!m_isDead)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float direction = Vector3.Dot(playerDirection, Vector3.right);

            if (playerDistance < 10)
            {
                m_combatIdle = false;

                if (direction > 0.1f)
                {
                    if (playerDistance > 3)
                    {
                        inputX = 0.6f;
                    }
                    else
                    {
                        inputX = 0.2f;
                    }
                }
                else if (direction < -0.1f)
                {
                    if (playerDistance > 3)
                    {
                        inputX = -0.6f;
                    }
                    else
                    {
                        inputX = -0.2f;
                    }
                }
                else
                {
                    inputX = 0f;
                }
            }

            if (playerDistance < 3f && playerDistance > 2.1f)
            {
                m_combatIdle = true;
            }

            if (playerDistance < 2f)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    m_animator.SetBool("Attack", false);
                }
                else {
                    m_animator.SetBool("Attack", true);
                }
            }
            else
            {
                m_combatIdle = false;
            }
        }

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_animator.SetFloat("RunSpeed", Mathf.Clamp(System.Math.Abs(inputX) * 1.5f ,0.7f, 1f));
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (enemy.returnDeathStatus())
        {
            m_isDead = true;
        }

        if (m_isDead)
        {
            m_animator.SetTrigger("Death");
        }
        else
        {
            m_animator.SetTrigger("Recover");
        }
        //Jump
        //else if (Input.GetKeyDown("space") && m_grounded) {
        //m_animator.SetTrigger("Jump");
        //m_grounded = false;
        //m_animator.SetBool("Grounded", m_grounded);
        // m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        // m_groundSensor.Disable(0.2f);
        //}

        //Run

        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float Speed;
    private EnvironmentController environmentController;

    private float PlayerHitCooldown;
    private bool PlayerHitOffCooldown;

    private bool isDmged;

    private Enemy enemy;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GameObject environment = GameObject.FindGameObjectWithTag("EnvironmentController");
        environmentController = environment.GetComponent<EnvironmentController>();
        animator = GetComponent<Animator>();

        enemy = GetComponent<Enemy>();
        PlayerHitCooldown = 2f;
        PlayerHitOffCooldown = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage and kb applied when basic attacked.
        if (collision.gameObject.tag != "Player") { return; }
        else
        {
            if (collision.gameObject.GetComponent<PlayerController>().GetAttackState() && PlayerHitOffCooldown)
            {
                GameObject player = collision.gameObject;

                enemy.TakeDamage(collision.gameObject.GetComponent<Player>().ReturnDmg());

                Vector3 playerDirection = (player.transform.position - transform.position).normalized;
                float direction = Vector3.Dot(playerDirection, Vector3.right);

                // Basic attack impact causes player to bounce back after impact.
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(5f * direction, 0f, 0f), ForceMode2D.Impulse);

                animator.SetTrigger("Hurt");
                StartCoroutine(BasicAttackInvulnerability());
            }
        }

        // Checks if enemy is dead and player in eatable state.
        if (collision.gameObject.GetComponent<PlayerController>().ReturnState() && enemy.ReturnDeathStatus())
        {
            DisableCollisions();
            StartCoroutine(EntityEaten());
        }
    }

    // Eats if player is still colliding (prevents soft locking if trapped on both sides)
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") { return; }
        else
        {
            if (collision.gameObject.GetComponent<PlayerController>().ReturnState() && enemy.ReturnDeathStatus())
            {
                StartCoroutine(EntityEaten());
            }
        }
    }

    IEnumerator EntityEaten()
    {
        float timeElapsed = 0f;
        float time = 2f;

        // Gains skill.
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.UpdateSkill(enemy.ReturnSkill());

        // Moves entity towards center of player whilst reducing it's scale.
        while (timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, GameObject.FindWithTag("Player").transform.position, timeElapsed / time);
            if (transform.localScale.x > 0.25f)
            {
                transform.localScale -= new Vector3(0.9f, 0.9f, 0.9f) * Time.deltaTime;
            }
            else if (transform.localScale.x < -0.25f) {
                transform.localScale -= new Vector3(-0.9f, 0.9f, 0.9f) * Time.deltaTime;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    // Prevents multiple attacks from one attack.
    IEnumerator BasicAttackInvulnerability()
    {
        ChangeBasicInvulState();
        yield return new WaitForSeconds(PlayerHitCooldown);
        ChangeBasicInvulState();
    }

    private void ChangeBasicInvulState()
    {
        PlayerHitOffCooldown ^= true;
    }

    public void DisableCollisions()
    {
        GetComponent<Rigidbody2D>().simulated = false;
    }

    public void RestoreCollisions()
    {
        GetComponent<Rigidbody2D>().simulated = true;
    }

}

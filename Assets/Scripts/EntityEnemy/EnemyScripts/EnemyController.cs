using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speed;
    private DifficultyController difficultyController;

    private float playerHitCooldown;
    private bool playerHitOffCooldown;

    private bool isDmged;
    private bool beingEaten;

    private Enemy enemy;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        difficultyController = GameObject.FindWithTag("DifficultyController").GetComponent<DifficultyController>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        // Increase enemy difficulty.
        int worldDifficulty = difficultyController.GetWorldDifficulty();

        enemy.SetDmg(worldDifficulty * enemy.ReturnDmg());
        enemy.IncreaseMaxHealth((int) Mathf.Round(worldDifficulty * enemy.ReturnMaxHP() * 0.4f));

        playerHitCooldown = 2f;
        playerHitOffCooldown = true;

        beingEaten = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage and kb applied when basic attacked.
        if (collision.gameObject.tag != "Player") { return; }
        else
        {
            if (collision.gameObject.GetComponent<PlayerController>().GetAttackState() && playerHitOffCooldown)
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

        // Checks if enemy is dead.
        if (enemy.ReturnDeathStatus())
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag != "Player") { return; }
        else if (collider.gameObject.GetComponent<PlayerController>().ReturnState() && enemy.ReturnDeathStatus())
        {
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
        if (!beingEaten)
        {
            beingEaten = true;
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
                else if (transform.localScale.x < -0.25f)
                {
                    transform.localScale -= new Vector3(-0.9f, 0.9f, 0.9f) * Time.deltaTime;
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);

            Destroy(gameObject);
        }
    }

    // Prevents multiple attacks from one attack.
    IEnumerator BasicAttackInvulnerability()
    {
        ChangeBasicInvulState();
        yield return new WaitForSeconds(playerHitCooldown);
        ChangeBasicInvulState();
    }

    private void ChangeBasicInvulState()
    {
        playerHitOffCooldown ^= true;
    }
}

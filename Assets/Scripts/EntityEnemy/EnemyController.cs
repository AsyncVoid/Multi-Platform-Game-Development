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

        enemy = GetComponent<Enemy>();
        PlayerHitCooldown = 2f;
        PlayerHitOffCooldown = true;

        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage and kb applied when attacked.
        if (collision.gameObject.tag != "Player") { return; }
        else
        {
            if (collision.gameObject.GetComponent<PlayerController>().GetAttackState() && PlayerHitOffCooldown)
            {
                GameObject player = collision.gameObject;

                enemy.TakeDamage(collision.gameObject.GetComponent<Player>().ReturnDmg());
                Debug.Log(enemy.ReturnHP());

                Vector3 playerDirection = (player.transform.position - transform.position).normalized;
                float direction = Vector3.Dot(playerDirection, Vector3.right);

                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(5f * direction, 0f, 0f), ForceMode2D.Impulse);

                animator.SetTrigger("Hurt");
                StartCoroutine(BasicAttackInvulnerability());
            }
        }

        // Gives Entity Skills to the player upon touched if player is in liquid state.
        if (collision.gameObject.GetComponent<PlayerController>().ReturnState() && gameObject.GetComponent<Enemy>())
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = gameObject.GetComponent<Enemy>();

            // if (!player.skills.Contains(enemy.ReturnSkill()))
                // player.skills.Add(enemy.ReturnSkill());

            StartCoroutine(EntityEaten());
        }

        // Begins the Destruction of the entity.
        else if (collision.gameObject.GetComponent<PlayerController>().ReturnState())
        {
            StartCoroutine(EntityEaten());
        }
    }

    IEnumerator EntityEaten()
    {

        float timeElapsed = 0f;
        float time = 2f;

        // Moves entity towards center of player whilst reducing it's scale.
        while (timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, GameObject.FindWithTag("Player").transform.position, timeElapsed / time);
            if (transform.localScale.x > 0.25f)
            {
                transform.localScale -= new Vector3(0.9f, 0.9f, 0.9f) * Time.deltaTime;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

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

    void Update()
    {
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

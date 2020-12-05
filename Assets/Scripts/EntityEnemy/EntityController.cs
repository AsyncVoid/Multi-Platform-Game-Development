using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private float Speed;
    private EnvironmentController environmentController;

    private float PlayerHitCooldown;
    private bool PlayerHitOffCooldown;

    private Enemy enemy = null;
    // Start is called before the first frame update
    void Start()
    {
        GameObject environment = GameObject.FindGameObjectWithTag("EnvironmentController");
        environmentController = environment.GetComponent<EnvironmentController>();

        enemy = GetComponent<Enemy>();
        PlayerHitCooldown = 2f;
        PlayerHitOffCooldown = true;
    }

    void OnCollisionEnter2D(Collision2D collision) {

        // Disables all collisions with the player if player collides with object.
        if (!collision.gameObject.CompareTag("Player")) 
            return;
        if (collision.gameObject.GetComponent<PlayerController>().GetAttackState() && PlayerHitOffCooldown && enemy != null)
        {
            enemy.TakeDamage(1);
            Debug.Log(enemy.ReturnHP());

            StartCoroutine(BasicAttackInvulnerability());
        }
        else {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().isTrigger = true;
        }

        // Gives Entity Skills to the player upon touched if player is in liquid state.
        if (collision.gameObject.GetComponent<PlayerController>().ReturnState() && gameObject.GetComponent<Enemy>()) {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = gameObject.GetComponent<Enemy>();

            // if (!player.skills.Contains(enemy.ReturnSkill()))
               // player.skills.Add(enemy.ReturnSkill());

            StartCoroutine(EntityEaten());
        }

        // Begins the Destruction of the entity.
        else if (collision.gameObject.GetComponent<PlayerController>().ReturnState()) {
            StartCoroutine(EntityEaten());
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag != "Player") { return; }
        else {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    IEnumerator EntityEaten() {

        float timeElapsed = 0f;
        float time = 2f;

        // Moves entity towards center of player whilst reducing it's scale.
        while (timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, GameObject.FindWithTag("Player").transform.position, timeElapsed / time);
            if (transform.localScale.x > 0.25f) { 
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

    private void ChangeBasicInvulState() {
        PlayerHitOffCooldown ^= true;
    }

    void FixedUpdate()
    {
    }

    public void DisableCollisions() {
        GetComponent<Rigidbody2D>().simulated = false;
    }

    public void RestoreCollisions() {
        GetComponent<Rigidbody2D>().simulated = true;
    }

}

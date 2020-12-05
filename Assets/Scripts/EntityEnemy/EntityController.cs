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

    private Enemy enemy;
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

        // Disables all collisions with the player if player collides with object. Allows player to move over object.
        if (collision.gameObject.tag != "Player") { return; }
        else 
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    // Trigger if player is one item and in state to eat.
    void onTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag != "Player") { return; }
        else if (collider.gameObject.GetComponent<PlayerController>().ReturnState())
        {
            StartCoroutine(EntityEaten());
        }
    }

    // Returns item into it's hittable state.
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

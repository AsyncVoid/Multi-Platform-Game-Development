using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public Skill skill;
    private Player player;
    private bool beingEaten;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        beingEaten = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {

        // Disables all collisions with the player / enemy if player collides with object. Allows player to move over object.
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EntityIgnore") { return; }
        else 
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().isTrigger = true;

        }
    }

    // Trigger if player is one item and in state to eat.
    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag != "Player") { return; }
        else if (collider.gameObject.GetComponent<PlayerController>().ReturnState())
        {
            StartCoroutine(EntityEaten());
        }
    }

    // Returns item into it's hittable state.
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag != "Player" && collider.gameObject.tag != "Enemy" && collider.gameObject.tag != "EntityIgnore") { return; }
        else {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    IEnumerator EntityEaten() {

        if (!beingEaten)
        {
            GetComponent<LockXAxis>().enabled = false;
            beingEaten = true;

            float timeElapsed = 0f;
            float time = 2f;

            player.UpdateSkill(skill);
            player.IncreaseMatter(2);
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

            Destroy(gameObject);
        }
    }
}

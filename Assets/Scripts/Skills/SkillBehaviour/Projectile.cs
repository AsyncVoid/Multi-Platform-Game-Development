using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ISkill
{
    // Start is called before the first frame update
    public float projectileSpeed;
    public float projectileDuration;
    public float projectileDropOff;
    public Vector3 projectileDirection;

    private Vector3 previousPos;
    private bool charged;

    private Animator animator;
    public GameObject sourceObject;
    private Skill skill;

    private bool hitConfirm;

    private DamageController damageController;

    
    void Start() {
        animator = GetComponent<Animator>();
        damageController = GameObject.FindWithTag("DamageController").GetComponent<DamageController>();

        // Ensures object dissapears after a certain time.
        StartCoroutine(DestroySelf());

        charged = false;
        hitConfirm = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if entity has an actual source. If none, doesn't change it's position.
        if (!(sourceObject is null))
        {
            // Checks if the start up animation has finished.
            if (!charged)
            {
                // If start up animation is not finished, moves the animation with the player when they move.
                if (sourceObject.tag == "Player")
                {
                    transform.position = sourceObject.transform.position;
                }
            }

            // Checks if animation has finished. If there's a flying animation after the start up animation, we'll have to change this code.
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                // Moves the projectile if the start up animation has ended.
                previousPos = transform.position;
                transform.position = transform.position + projectileDirection * Time.deltaTime * projectileSpeed;

                // Ensures the projectile keeps facing the right direction.
                projectileDirection = new Vector3(projectileDirection.x, projectileDirection.y + projectileDropOff * Time.deltaTime, projectileDirection.z);

                // Tells us the animation has ended.
                charged = true;
            }
        }
    }

    void FixedUpdate()
    {
        // Keeps projectile rotation facing the right direction.
        float dirAngle = Vector3.SignedAngle(projectileDirection, Vector3.up, Vector3.forward);
        transform.eulerAngles = new Vector3 (0f, 0f, 90 - dirAngle);
    }

    // Destroys itself after a certain time frame. Also plays the destruction of projectile animation.
    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(projectileDuration);
        animator.SetTrigger("Disintegrate");

        yield return new WaitForSeconds(0.5f);

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    // Coroutine for destruction after collision, makes sure animation finishes before destruction.
    IEnumerator DestroySelfCollision() {
        animator.SetTrigger("Disintegrate");

        yield return new WaitForSeconds(0.5f);

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    // Method for the interface ISkill, Put logic to start up a prefab of the skill.
    public void UseSkill(Skill skillUsed, GameObject entity, Vector3 targetDirection) {
        if (entity.tag == "Enemy")
        {
            // Witch has projectile spell.
            Projectile selfRef = Instantiate(this, entity.transform.Find("Staff").position, Quaternion.identity);

            // Needs to assign variables this way to the initialised prefab.
            selfRef.GetComponent<Projectile>().projectileDropOff = 0f;
            selfRef.GetComponent<Projectile>().skill = skillUsed;
            selfRef.GetComponent<Projectile>().projectileDirection = targetDirection;
            selfRef.GetComponent<Projectile>().sourceObject = entity;
        }
        else
        {
            Projectile selfRef = Instantiate(this, entity.transform.position, Quaternion.identity);

            // Needs to assign variables this way to the initialised prefab.
            selfRef.GetComponent<Projectile>().skill = skillUsed;
            selfRef.GetComponent<Projectile>().projectileDirection = targetDirection;
            selfRef.GetComponent<Projectile>().sourceObject = entity;
        }
    }

    // Projectile hit method.
    private void OnCollisionEnter2D(Collision2D other) {
        GameObject entityHit = other.gameObject;

        // Different effects for hitting different objects.

        // Ensures object doesn't die when being fired from the entity.
        if (!hitConfirm)
        {
            if (entityHit == sourceObject) { return; }

            // Destroy on ground collision.
            else if (entityHit.tag == "Tilemap")
            {
                StartCoroutine(DestroySelfCollision());
            }

            // If an enemy uses this skill and hits a player, following is ran.
            else if (entityHit.tag == "Player")
            {
                if (entityHit.gameObject.GetComponent<PlayerController>().ReturnState())
                {
                    StartCoroutine(Eaten());
                    hitConfirm = true;
                }
                else
                {
                    StartCoroutine(DestroySelfCollision());
                    damageController.DamagePlayer(sourceObject.GetComponent<Enemy>());
                    hitConfirm = true;
                }
            }

            // If player hits enemy (potential enemy friendly fire).
            else if (entityHit.tag == "Enemy")
            {
                StartCoroutine(DestroySelfCollision());
                damageController.DamageEnemy(skill, entityHit.GetComponent<Enemy>());
                hitConfirm = true;
            }
        }
    }

    // Makes sure the object can leave the source object without interfering with rigidbody collisions.
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == sourceObject) {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Tilemap") {
            StartCoroutine(DestroySelfCollision());
        }
    }

    IEnumerator Eaten() {
        float timeElapsed = 0f;
        float time = 2f;

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().isTrigger = true;

        GameObject.FindWithTag("Player").GetComponent<Player>().IncreaseMatter(2);
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

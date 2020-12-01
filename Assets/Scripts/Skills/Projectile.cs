using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float projectileSpeed;
    public float projectileDuration;
    public Vector3 projectileDirection;

    private Vector3 previousPos;
    private bool charged;

    private Animator animator;
    public GameObject sourceObject;

    private DamageController damageController;
    public Skill skill;

    void Start()
    {
        projectileSpeed = 8.0f;
        projectileDuration = 3.2f;
        animator = GetComponent<Animator>();

        damageController = GameObject.FindWithTag("DamageController").GetComponent<DamageController>();

        StartCoroutine(DestroySelf());

        charged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!charged) {
            transform.position = GameObject.FindWithTag("Player").transform.position;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            previousPos = transform.position;
            transform.position = transform.position + projectileDirection * Time.deltaTime * projectileSpeed;
            projectileDirection = new Vector3(projectileDirection.x, projectileDirection.y - 0.25f * Time.deltaTime, projectileDirection.z);
            charged = true;
        }
    }

    void FixedUpdate()
    {
        float dirAngle = Vector3.SignedAngle(projectileDirection, Vector3.up, Vector3.forward);
        transform.eulerAngles = new Vector3 (0f, 0f, 90 - dirAngle);
    }

    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(projectileDuration);
        animator.SetTrigger("Disintegrate");

        yield return new WaitForSeconds(0.5f);

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    IEnumerator DestroySelfCollision() {
        animator.SetTrigger("Disintegrate");

        yield return new WaitForSeconds(0.5f);

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameObject entityHit = other.gameObject;

        if (entityHit == sourceObject) { return; }
        else if (entityHit.tag == "Tilemap")
        {
            StartCoroutine(DestroySelfCollision());
        }
        else if (entityHit.tag == "Player") {
            StartCoroutine(DestroySelfCollision());
            damageController.DamagePlayer(sourceObject.GetComponent<Enemy>());
        }
        else if (entityHit.tag == "Enemy")
        {
            StartCoroutine(DestroySelfCollision());
            damageController.DamageEnemy(skill, entityHit.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == sourceObject) {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}

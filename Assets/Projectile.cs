using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float projectileSpeed;
    public float projectileDuration;
    private Vector3 projectileDirection;

    private Vector3 previousPos;
    private bool charged;

    private Animator animator;

    void Start()
    {
        projectileSpeed = 8.0f;
        projectileDuration = 3.2f;
        animator = GetComponent<Animator>();

        StartCoroutine(DestroySelf());

        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector3 targetPosition = new Vector3(worldPosition.x, worldPosition.y, 0.0f);

        projectileDirection = (targetPosition - transform.position).normalized;

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
        string entityHit = other.gameObject.tag;

        if (entityHit == "Player") { return; }
        else {
            StartCoroutine(DestroySelfCollision());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}

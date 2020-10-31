using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private float Speed;
    private Vector3 ItemVelocity;
    private EnvironmentController environmentController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject environment = GameObject.FindGameObjectWithTag("EnvironmentController");
        environmentController = environment.GetComponent<EnvironmentController>();
        Speed = environmentController.GetMovementSpeed();
        Debug.Log("Speed: " + Speed);
        ItemVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Player") { return; }
        else {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().isTrigger = true;
        }

        if (collision.gameObject.GetComponent<PlayerController>().ReturnState()) {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = gameObject.GetComponent<Enemy>();
            if(!player.skills.Contains(enemy.ReturnSkill()))
                player.skills.Add(enemy.ReturnSkill());
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
        ItemVelocity = new Vector3(0f, 0f, 0f);

        float timeElapsed = 0f;
        float time = 2f;

        while (timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, GameObject.FindWithTag("Player").transform.position, timeElapsed / time);
            transform.localScale -= new Vector3(0.9f, 0.9f, 0.9f) * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    private void Update()
    {
        //Speed = environmentController.GetMovementSpeed();
        //ItemVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    void FixedUpdate()
    {
        transform.position += ItemVelocity * Time.deltaTime;
    }

    public void HaltEntityMovement()
    {
        ItemVelocity = new Vector3(0, 0, 0);
    }

    public void RestoreEntityMovement()
    {
        ItemVelocity = new Vector3(-1 * Speed, 0, 0);
    }

    public void DisableCollisions() {
        GetComponent<Rigidbody2D>().simulated = false;
    }

    public void RestoreCollisions() {
        GetComponent<Rigidbody2D>().simulated = true;
    }

}

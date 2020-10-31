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
            GetComponent<Rigidbody2D>().simulated = false; 
        }

        if (collision.gameObject.GetComponent<PlayerController>().ReturnState()) {
            StartCoroutine(EntityEaten());
        }
    }

    IEnumerator EntityEaten() {
        ItemVelocity = new Vector3(-0.2f, 0, 0);

        while (transform.localScale.x > 0.45f)
        {
            transform.localScale -= new Vector3(0.9f, 0.9f, 0.9f) * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
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

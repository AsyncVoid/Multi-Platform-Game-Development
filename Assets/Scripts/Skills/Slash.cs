using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour, ISkill
{
    // Start is called before the first frame update
    private Skill skill;
    private Vector3 slashPlacement;
    private bool hitConfirm;
    private float leftOrRight;

    private GameObject sourceObject;
    private DamageController damageController;

    void Start()
    {
        hitConfirm = false;
        damageController = GameObject.FindWithTag("DamageController").GetComponent<DamageController>();
        StartCoroutine(DestroySelfAfterTime());
    }

    void Update() {
        if (leftOrRight < 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 1.0f);
        }
        else {
            transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
        }
    }

    // Method for the interface ISkill, Put logic to start up a prefab of the skill.
    public void UseSkill(Skill skillUsed, GameObject entity, Vector3 targetDirection)
    {
        // Check for direction to place the slash attack.
        leftOrRight = Vector3.Dot(targetDirection, Vector3.right);

        if (leftOrRight > 0)
        {
            slashPlacement = new Vector3(entity.transform.position.x + 1.3f, entity.transform.position.y, entity.transform.position.z);
        }
        else {
            slashPlacement = new Vector3(entity.transform.position.x - 1.3f, entity.transform.position.y, entity.transform.position.z);
        }

        // Instantiate the slash collider prefabs and assign variables.
        Slash selfRef = Instantiate(this, slashPlacement, Quaternion.identity);

        selfRef.GetComponent<Slash>().skill = skillUsed;
        selfRef.GetComponent<Slash>().sourceObject = entity;
        selfRef.GetComponent<Slash>().leftOrRight = leftOrRight;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject entityHit = other.gameObject;

        if (!hitConfirm)
        {
            if (entityHit == sourceObject)
            {
                return;
            }
            else if (entityHit.tag == "Enemy")
            {
                Enemy enemy = entityHit.GetComponent<Enemy>();
                damageController.DamageEnemy(skill, enemy);
                hitConfirm = true;
            }
            else if (entityHit.tag == "Player")
            {
                damageController.DamagePlayer(sourceObject.GetComponent<Enemy>());
                hitConfirm = true;
            }
        }
    }

    IEnumerator DestroySelfAfterTime() {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

}

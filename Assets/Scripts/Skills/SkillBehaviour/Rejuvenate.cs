using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rejuvenate : MonoBehaviour, ISkill
{
    private DamageController damageController;
    private GameObject sourceObject;
    private bool healingStarted;
    private Skill skill;
    // Start is called before the first frame update
    void Start()
    {
        damageController = GameObject.FindWithTag("DamageController").GetComponent<DamageController>();
        healingStarted = false;
    }

    void Update() {

        if (!healingStarted) {
            if (sourceObject.tag == "Player")
            {
                StartCoroutine(PlayerHealOverTime(skill));
            }
            else if (sourceObject.tag == "Enemy")
            {
                Enemy enemy = sourceObject.GetComponent<Enemy>();
                StartCoroutine(EnemyHealOverTime(enemy));
            }
        }
    }

    // Check which unit type used healing.
    public void UseSkill(Skill skillUsed, GameObject entity, Vector3 targetDirection)
    {

        Rejuvenate selfRef = Instantiate(this, entity.transform.position, Quaternion.identity);

        selfRef.GetComponent<Rejuvenate>().sourceObject = entity;
        selfRef.GetComponent<Rejuvenate>().skill = skillUsed;
    }

    // Healing over time.
    IEnumerator PlayerHealOverTime(Skill skill) {
        healingStarted = true;

        for (int i = 0; i < 3; i++) {
            damageController.HealPlayer(skill);
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(gameObject);
    }

    IEnumerator EnemyHealOverTime(Enemy enemy) {
        healingStarted = true;

        for (int i = 0; i < 3; i++)
        {
            damageController.HealEnemy(enemy);
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(gameObject);
    }
}

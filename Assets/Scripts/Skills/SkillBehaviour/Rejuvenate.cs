using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rejuvenate : MonoBehaviour, ISkill
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseSkill(Skill skillUsed, GameObject entity, Vector3 targetDirection)
    {
        if (entity.tag == "Player")
        {
            Player player = entity.GetComponent<Player>();

        }
        else if (entity.tag == "Enemy")
        {
            Enemy enemy = entity.GetComponent<Enemy>();
        }
    }
}

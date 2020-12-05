using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    void UseSkill(Skill skill, GameObject sourceEntity, Vector3 targetPosition);
}

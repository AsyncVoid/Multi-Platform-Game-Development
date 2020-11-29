using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit

{
    public List<Skill> skills;
    public int maxMatter;
    public int matter;
    public float matterRegenRate;

    void Start() 
    {
        maxMatter = 10;
        matter = 0;
        matterRegenRate = 5.0f;

        StartCoroutine(MatterRegeneration());
    }

    public int ReturnMaxMatter() {
        return maxMatter;
    }

    public int ReturnMatter() {
        return matter;
    }

    IEnumerator MatterRegeneration()
    {
        if (matter < maxMatter){
            matter += 1;
        }

        yield return new WaitForSeconds(matterRegenRate);
        StartCoroutine(MatterRegeneration());
    }
}


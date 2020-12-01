using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit

{
    public int maxMatter;
    public int matter;
    public float matterRegenRate;

    public SkillList skills;

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

    public bool UseMatter(int matterUsage) {
        if (matter < matterUsage)
        {
            return false;
        }
        else {
            matter -= matterUsage;
            return true;
        }
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


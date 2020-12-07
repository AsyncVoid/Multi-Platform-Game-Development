using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Unit

{
    public int maxMatter;
    public int matter;
    public float matterRegenRate;

    public SkillList skills;
	public float delay = 15f;
    private GameObject playerModel;
    private PlayerController playerController;
    public Animator animator;
    private SceneHandler sceneHandler;

    void Start() 
    {
        sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
        playerModel = GameObject.FindWithTag("PlayerModel");
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        maxMatter = 10;
        matter = 0;
        matterRegenRate = 5.0f;

        StartCoroutine(MatterRegeneration());

    }

    void Update()
    {
        if(ReturnDeathStatus()){
            Death();
        }
    }

    public int ReturnMaxMatter() {
        return maxMatter;
    }

    public int ReturnMatter() {
        return matter;
    }

    // For using skills, matter is consumed. Returns false if not enough matter is present.
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

    public void UpdateSkill(Skill skill) {
        skills.AddSkill(skill);
    }

    // Passive matter regeneration coroutine.
    IEnumerator MatterRegeneration()
    {
        if (matter < maxMatter){
            matter += 1;
        }

        yield return new WaitForSeconds(matterRegenRate);
        StartCoroutine(MatterRegeneration());
    }

    void Death()
    {
        if(animator.GetBool("leftHeld")){
            animator.SetBool("leftHeld", false);
            animator.SetTrigger("animationEnd");

        }
        else if(animator.GetBool("rightHeld")){
            animator.SetBool("rightHeld", false);
            animator.SetTrigger("animationEnd");
        }
        else{
            animator.SetBool("leftHeld", false);
            animator.SetBool("rightHeld", false);
            animator.SetTrigger("animationEnd");
        }
        animator.SetBool("dead", true);
        StartCoroutine(deathAnimation());

        //Debug.Log("You have died.");
        sceneHandler.DeathScene();
        

    }

    IEnumerator deathAnimation() 
    {
        yield return new WaitForSeconds(15f);

    }

}


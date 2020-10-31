using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CombatState { Start, PlayerTurn, EnemyTurn, Won, Lost }
public class CombatSystem : MonoBehaviour
{
    //public Transform playerTransform;
    //public Transform enemyTransform;
    Player player;
    Enemy enemy;
    public HealthBar playerHealth;
    public HealthBar enemyHealth;
    public CombatState state;
    public Text dialogueText;
    public GameObject combatHUD;
    public GameObject attackButton;
    public GameObject[] skillButtons;

    private EnvironmentController EnvironmentController;

    void Start()
    {
        combatHUD.gameObject.SetActive(false);
        EnvironmentController = GameObject.FindWithTag("EnvironmentController").GetComponent<EnvironmentController>();
    }

    public void StartBattle(Player player, Enemy enemy)
    {
        this.player = player;
        this.enemy = enemy;
        player.GetComponent<PlayerController>().enabled = false;
        state = CombatState.Start;
        StartCoroutine(SetupCombat());

    }
    public void SetSkills(List<Skill> skills) {
        skillButtons = GameObject.FindGameObjectsWithTag("Skill");
        for(int i =0;i<player.skills.Count; i++){
            skillButtons[i].GetComponent<Image>().sprite = player.skills[i].skillSprite;
            skillButtons[i].GetComponent<Image>().color = Color.white;
            skillButtons[i].GetComponent<SkillButton>().skill = player.skills[i];
        }
    }

    IEnumerator SetupCombat()
    {
        combatHUD.gameObject.SetActive(true);
        player = player.GetComponent<Player>();
        enemy = enemy.GetComponent<Enemy>();

        dialogueText.text = player.unitName + " has encountered " + enemy.unitName;

        playerHealth.SetHUD(player);
        enemyHealth.SetHUD(enemy);
        SetSkills(player.skills);

        yield return new WaitForSeconds(1f);

        state = CombatState.PlayerTurn; // Start player turn
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Player's turn: Choose an action!";
    }

    public void OnAttackButton()
    {
        if (state != CombatState.PlayerTurn)
            return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemy.TakeDamage(player.damage);

        enemyHealth.SetHP(enemy.ReturnHP());
        dialogueText.text = player.unitName + " has attacked " + enemy.unitName;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            enemy.isDead = true;
            state = CombatState.Won;
            enemy.tag = "Untagged";
            StartCoroutine(EndBattle());
        }
        else
        {
            state = CombatState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnSkillButton()
    {
        if (state != CombatState.PlayerTurn)
            return;
        
        StartCoroutine(PlayerSkill());

    }

    IEnumerator PlayerSkill()
    {
        SkillButton buttonClicked = EventSystem.current.currentSelectedGameObject.GetComponent<SkillButton>();
        Skill skillUsed = buttonClicked.skill;

        bool isDead = enemy.TakeDamage(skillUsed.skillDamage);
        enemyHealth.SetHP(enemy.ReturnHP());
        dialogueText.text = player.unitName + " used " + skillUsed.skillName + " on " + enemy.unitName;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            enemy.isDead = true;
            state = CombatState.Won;
            enemy.tag = "Untagged";
            enemy.GetComponent<Animator>().Play("Base Layer.Death Animation");
            StartCoroutine(EndBattle());
        }
        else
        {
            state = CombatState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemy.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = player.TakeDamage(enemy.damage);
        playerHealth.SetHP(player.ReturnHP());

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = CombatState.Lost;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = CombatState.PlayerTurn;
            PlayerTurn();
        }

    }


    IEnumerator EndBattle()
    {
        if (state == CombatState.Won)
            dialogueText.text = "You've defeated the " + enemy.unitName;
        else if (state == CombatState.Lost)
            dialogueText.text = "You have been defeated";

        yield return new WaitForSeconds(1f);


        combatHUD.gameObject.SetActive(false);

        player.GetComponent<PlayerController>().enabled = true;

        EnvironmentController.EndCombatScene();

    }

}
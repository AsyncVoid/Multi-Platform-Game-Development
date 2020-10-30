using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Button AttackButton;
    public Button PlayerSkill;
    
    void Start()
    {
        combatHUD.gameObject.SetActive(false);
    }

    public void StartBattle(Player player, Enemy enemy)
    {
        this.player = player;
        this.enemy = enemy;
        player.GetComponent<PlayerController>().enabled = false;
        state = CombatState.Start;
        StartCoroutine(SetupCombat());
        
    }

    IEnumerator SetupCombat() 
    {
        combatHUD.gameObject.SetActive(true);
        player = player.GetComponent<Player>();
        enemy = enemy.GetComponent<Enemy>();

        dialogueText.text = player.unitName + " has encountered " + enemy.unitName;

        playerHealth.SetHUD(player);
        enemyHealth.SetHUD(enemy);

        enemy.transform.position = new Vector3(9.5f,-0.5f,0);

        yield return new WaitForSeconds(2f);

        state = CombatState.PlayerTurn; // Start player turn
        PlayerTurn();
    
    }

    void PlayerTurn()
    {
        dialogueText.text = "Player's turn: Choose an action!";
        AttackButton.onClick.AddListener(OnAttackButton);        
    }

	public void OnAttackButton()
	{
		if (state != CombatState.PlayerTurn)
			return;

		StartCoroutine(PlayerAttack());
	}

    public void OnSkillButton()
    {
        int numSkills = player.skills.Count;

        if(numSkills == 0)
            dialogueText.text = "You have not acquired any skills.";
        else{
        }
    }

    IEnumerator PlayerAttack()
    {
		bool isDead = enemy.TakeDamage(player.damage);

        enemyHealth.SetHP(enemy.ReturnHP());
        dialogueText.text = player.unitName + " has attacked " + enemy.unitName;

		yield return new WaitForSeconds(2f);

		if(isDead)
		{
            enemy.isDead = true;
			state = CombatState.Won;
			EndBattle();
		} else
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

		if(isDead)
		{
			state = CombatState.Lost;
			EndBattle();
		} 
        else
		{
			state = CombatState.PlayerTurn;
			PlayerTurn();
		}

	}


	void EndBattle()
	{
		if(state == CombatState.Won)
            dialogueText.text = "You've defeated the " + enemy.unitName;
        else if(state == CombatState.Lost)
            dialogueText.text = "You have been defeated";
        combatHUD.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;
	}

}

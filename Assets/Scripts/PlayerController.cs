using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private float Speed;
    private Rigidbody2D rb;

    private bool Liquified;
    private bool LiquifiedOffCooldown;

    private int LiquifiedLength;
    private int LiquifiedCooldown;
    public GameObject player;
    public GameObject entity;
    private Renderer playerRender;

    public CombatSystem combatSystem; 
    public CombatState state;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerRender = player.GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
        Speed = 5;
        
        Liquified = false;
        LiquifiedLength = 2;
        LiquifiedCooldown = 5;
        LiquifiedOffCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) 
        {
            if (LiquifiedOffCooldown)
            {
                StartCoroutine(TimedStateChange()); 
            }
        }

        if(ReturnState()){
            playerRender.material.SetColor("_Color",Color.green);
        }
        else{
            playerRender.material.SetColor("_Color",Color.white);
        }
    }

    void FixedUpdate() 
    {
        float translation = Input.GetAxis("Horizontal") * Speed;

        if (translation != 0){
            transform.Translate(translation * Time.deltaTime, 0, 0);
        }
    }

    private void ChangeLiquidState() 
    {
        Liquified ^= true;
    }

    private void UpdateLiquifiedCooldown() 
    {
        LiquifiedOffCooldown ^= true;
    }

    IEnumerator TimedStateChange() 
    {
        ChangeLiquidState();
        UpdateLiquifiedCooldown();
        yield return new WaitForSeconds(LiquifiedLength);
        ChangeLiquidState();
        yield return new WaitForSeconds(LiquifiedCooldown);
        UpdateLiquifiedCooldown();
    }

    public bool ReturnState() {
        return Liquified;
    }

    public bool ReturnCooldown() {
        return LiquifiedOffCooldown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CombatSystem combat = combatSystem.GetComponent<CombatSystem>();
        entity = other.gameObject.gameObject;

        // If player collides with enemy and enemy is dead
        if(other.gameObject.tag == "Enemy" && entity.GetComponent<Unit>().isDead){
            if(player.GetComponent<PlayerController>().ReturnState()){
                Skill skill = entity.GetComponent<Enemy>().ReturnSkill();
                player.GetComponent<Player>().skills.Add(skill);
                GameObject.Destroy(entity);  
            }
        }
        else{
            combat.StartBattle(player.gameObject.GetComponent<Player>(), other.gameObject.GetComponent<Enemy>());
        }


    }
}
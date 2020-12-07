using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private DifficultyController difficultyController;

    public Text score;
    public Text worldDifficulty;

    public int playerScore;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        difficultyController = GameObject.FindWithTag("DifficultyController").GetComponent<DifficultyController>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        int progressCounter = 0;

        foreach (Skill skill in player.skills.skills)
        {
            progressCounter += skill.skillProgression;
        }

        playerScore = progressCounter * (player.ReturnMaxHP() + player.ReturnMaxMatter());

        score.text = "Score: " + playerScore.ToString();
        worldDifficulty.text = "World Difficulty: " + difficultyController.GetWorldDifficulty().ToString();

        //if (player.ReturnDeathStatus()) {
                 //  }
    }
}
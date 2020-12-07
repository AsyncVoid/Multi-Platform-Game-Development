using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private DifficultyController difficultyController;

    public Text score;
    public Text worldDifficulty;
    public Text overText;

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

        if (player.ReturnDeathStatus()) {
            overText.text = "You died with score: " + playerScore.ToString() + "     Press 'L' to restart.";
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Time.timeScale = 1f;

            foreach (Skill skill in player.skills.skills)
            {
                skill.skillTier = 0;
                skill.skillProgression = 0;
            }

            player.skills.skills.Clear();
            overText.text = "";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    private int worldDifficulty;
    private int progressCounter;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        worldDifficulty = 1;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        progressCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        progressCounter = 0;

        foreach (Skill skill in player.skills.skills) {
            progressCounter += skill.skillProgression;
        }

        switch (progressCounter) {
            case var _ when progressCounter <= 10:
                worldDifficulty = 1;
                break;
            case var _ when progressCounter <= 25:
                worldDifficulty = 2;
                break;
            case var _ when progressCounter <= 50:
                worldDifficulty = 3;
                break;
            default:
                worldDifficulty = 4;
                break;
        }
    }

    public int GetWorldDifficulty() {
        return worldDifficulty;
    }
}

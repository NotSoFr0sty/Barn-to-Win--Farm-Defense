using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject pauseScreen;
    public UnityEvent pauseGame;
    public UnityEvent resumeGame;
    private bool pause = false;
    private bool gameIsOver = false;

    // Stage management
    public GameObject[] stage;
    private GameObject currentStage;
    private int currentStageIndex;
    public int maxDifficulty = 3;
    public int[] scoreRequirementForClearingStage;
    public ScoreTracker scoreTracker;
    private int score;
    [SerializeField] private bool skipTutorial = false;
    public int skipToStage = 0;


    // Start is called before the first frame update
    void Start() {

        if (skipTutorial) {

            skipToStage = 1;
        }
        else currentStageIndex = skipToStage;

        currentStage = stage[currentStageIndex];

        // Set the currentStage active
        if (currentStage != null) currentStage.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

        // Pause/resume the game with Esc
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !gameIsOver) {

            pause = !pause;
            //Debug.Log(pause);

            if (pause) {
                // Pause the game
                PauseGame();
            }
            else {
                // Resume the game
                ResumeGame();
            }
        }

        // Track score
        if (scoreTracker != null) score = scoreTracker.score;

        if (currentStageIndex == 1) {

            if (score >= scoreRequirementForClearingStage[1]) {

                AdvanceToNextStage();
            }
        }

    }

    public void SetGameOver() {

        gameIsOver = true;
    }

    public void RestartGame() {

        SceneManager.LoadScene(0);
        ResumeGame();
    }

    public void PauseGame() {

        pauseGame.Invoke();
        pause = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame() {

        resumeGame.Invoke();
        pause = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void AdvanceToNextStage() {
        // Advances game to next stage

        // If maxDifficulty is reached or if the next stage is null, return false
        if (currentStageIndex >= maxDifficulty || stage[currentStageIndex + 1] == null) {
            return;
        }

        // Deactivate currentStage
        if (currentStage != null) currentStage.SetActive(false);

        // Advance to next stage
        currentStageIndex++;
        currentStage = stage[currentStageIndex];

        // Set the currentStage active
        if (currentStage != null) currentStage.SetActive(true);

    }

}

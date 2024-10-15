using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private GameObject currentStageGameObject;
    private int currentStageIndex;
    public int maxDifficulty = 3;
    public int[] scoreRequirementForClearingStage;
    public ScoreTracker scoreTracker;
    private int score;
    [SerializeField] private static bool skipTutorial = false;
    public int skipToStage = 0;
    public TMP_Text stageClearedText;
    public HealthController barnHealthController;
    public float healAmountPercentage = 50;


    // Start is called before the first frame update
    void Start() {

        if (skipTutorial) {

            skipToStage = 1;
        }
        currentStageIndex = skipToStage;
        currentStageGameObject = stage[currentStageIndex];

        // Set score according to skipToStage
        if (skipToStage > 1) {
            scoreTracker.score = scoreRequirementForClearingStage[skipToStage - 1];
        }

        // Set the currentStage active
        if (currentStageGameObject != null) currentStageGameObject.SetActive(true);
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

        if (currentStageIndex != 0 && currentStageIndex < stage.Length && currentStageIndex < scoreRequirementForClearingStage.Length) {

            if (score >= scoreRequirementForClearingStage[currentStageIndex]) {

                AdvanceToNextStage();
            }
        }

    }

    public void SetGameOver() {

        gameIsOver = true;
    }

    public void RestartGame() {

        skipTutorial = true;
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
        StartCoroutine("AdvanceToNextStageCoroutine");
    }

    IEnumerator AdvanceToNextStageCoroutine() {
        // Advances game to next stage

        // // If maxDifficulty is reached or if the next stage is null, return false
        // if (currentStageIndex >= maxDifficulty || stage[currentStageIndex + 1] == null) {
        //     return;
        // }

        if (currentStageIndex < (stage.Length - 1)) {

            // Deactivate currentStage
            if (currentStageGameObject != null) currentStageGameObject.SetActive(false);

            // Advance to next stage
            currentStageIndex++;
            currentStageGameObject = stage[currentStageIndex];

            //TODO: Kill all clones

            if (currentStageIndex > 1) {
                // Stage cleared text
                stageClearedText.gameObject.SetActive(true);
                stageClearedText.text = String.Format("LEVEL {0} CLEARED", currentStageIndex - 1);
                yield return new WaitForSeconds(2.0f);

                // heal barn 50% of missing health
                bool barnHealed = barnHealthController.healForXPercentOfMissingHealth(healAmountPercentage);

                if (barnHealed) stageClearedText.text += "\nBARN HEALED";
                yield return new WaitForSeconds(2.0f);
                stageClearedText.gameObject.SetActive(false);
            }
            else {
                // Tutorial finished text
                stageClearedText.gameObject.SetActive(true);
                stageClearedText.text = String.Format("DEFEND THE BARN", currentStageIndex - 1);

                // heal barn fully
                bool barnHealed = barnHealthController.healForXPercentOfMissingHealth(100);
                yield return new WaitForSeconds(3.0f);
                stageClearedText.gameObject.SetActive(false);
            }

            // Set the currentStage active
            if (currentStageGameObject != null) currentStageGameObject.SetActive(true);

        }
    }

}

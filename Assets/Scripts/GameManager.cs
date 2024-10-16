using System;
using System.Collections;
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
    public HealthController playerHealthController;
    public float healAmountPercentage = 50;
    public GameObject mooseWarning;
    public int numberOfBlinks = 4;
    public float blinkDurationOn = 0.30f;
    public float blinkDurationOff = 0.20f;


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

            if (currentStageIndex > 1) {
                // make player invincible
                playerHealthController.isInvincible = true;

                // Stage cleared text
                stageClearedText.gameObject.SetActive(true);
                stageClearedText.text = String.Format("LEVEL {0} CLEARED", currentStageIndex - 1);
                yield return new WaitForSeconds(2.0f);

                // Destroy all farmAnimals
                GameObject[] farmAnimals = GameObject.FindGameObjectsWithTag("FarmAnimal");
                foreach (GameObject animal in farmAnimals) {
                    Destroy(animal);
                }

                // Fully heal player
                playerHealthController.healForXPercentOfMissingHealth(100);

                // heal barn 50% of missing health
                bool barnHealed = barnHealthController.healForXPercentOfMissingHealth(healAmountPercentage);

                if (barnHealed) stageClearedText.text += "\nBARN HEALED";
                yield return new WaitForSeconds(2.0f);
                stageClearedText.gameObject.SetActive(false);

                // make player mortal again
                playerHealthController.isInvincible = false;
            }
            else {
                // Tutorial finished text
                stageClearedText.gameObject.SetActive(true);
                stageClearedText.text = String.Format("DEFEND THE BARN", currentStageIndex - 1);

                // Fully heal player
                playerHealthController.healForXPercentOfMissingHealth(100);

                // heal barn fully
                bool barnHealed = barnHealthController.healForXPercentOfMissingHealth(100);
                yield return new WaitForSeconds(3.0f);
                scoreTracker.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                stageClearedText.gameObject.SetActive(false);
            }

            // Set the currentStage active
            if (currentStageGameObject != null) currentStageGameObject.SetActive(true);

        }
    }

    public IEnumerator WarnAboutMoose() {

        for (int i = 0; i < numberOfBlinks; i++) {

            mooseWarning.SetActive(true);
            yield return new WaitForSeconds(blinkDurationOn);

            mooseWarning.SetActive(false);
            yield return new WaitForSeconds(blinkDurationOff);
        }
    }

}

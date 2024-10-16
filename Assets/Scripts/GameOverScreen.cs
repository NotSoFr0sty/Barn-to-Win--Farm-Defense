using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOverScreen : MonoBehaviour {

    public TMP_Text gameOverScoreText;
    public TMP_Text highscoreText;
    public TMP_Text messageText;
    private ScoreTracker scoreTracker;
    public GameObject restartButton;
    public bool gameOverCalled = false;
    public UnityEvent gameOver;
    public UnityEvent restartGame;
    [SerializeField] float restartDelay = 2.0f;
    private bool canRestart = false;
    int finalScore;

    // Start is called before the first frame update
    void Start() {

        restartButton.SetActive(false);
        StartCoroutine("Delay");
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && canRestart) {

            restartGame.Invoke();
        }
    }

    public void StartGameOverScreen(string message = "MESSAGE") {

        if (gameOverCalled) return;

        gameOver.Invoke();
        gameOverCalled = true;
        gameObject.SetActive(true);

        scoreTracker = Component.FindObjectOfType<ScoreTracker>();
        if (scoreTracker == null) return;
        finalScore = scoreTracker.score;
        gameOverScoreText.text = "SCORE: " + finalScore;
        messageText.text = message;

        // Now, disable the ScoreTracker gameObject
        scoreTracker.gameObject.SetActive(false);

        // Debug.Log(String.Format("Highscore: {0}", PlayerPrefs.GetInt("Highscore")));

        // If the score > highscore, then set score as the new highscore
        if (finalScore > PlayerPrefs.GetInt("Highscore")) {

            PlayerPrefs.SetInt("Highscore", finalScore);
            highscoreText.text = "NEW HIGHSCORE: " + finalScore;
            StartCoroutine("FlashHighscoreText");
        }
        else highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore");


    }

    IEnumerator FlashHighscoreText() {

        for (int i = 0; i < 5; i++) {

            highscoreText.GetComponent<CanvasGroup>().alpha = 0;
            yield return new WaitForSeconds(0.25f);

            highscoreText.GetComponent<CanvasGroup>().alpha = 1;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator Delay() {

        yield return new WaitForSeconds(restartDelay);
        canRestart = true;
        if (restartButton != null) restartButton.SetActive(true);
    }

}

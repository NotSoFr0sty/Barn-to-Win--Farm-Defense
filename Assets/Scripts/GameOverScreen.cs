using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

    public TMP_Text gameOverScoreText;
    public TMP_Text messageText;
    private ScoreTracker scoreTracker;
    public GameObject restartButton;
    public bool gameOverCalled = false;
    public UnityEvent gameOver;
    public UnityEvent restartGame;
    [SerializeField] float restartDelay = 2.0f;
    private bool canRestart = false;

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
        gameOverScoreText.text = "SCORE: " + scoreTracker.score;
        messageText.text = message;

        // Now, disable the ScoreTracker gameObject
        scoreTracker.gameObject.SetActive(false);


    }

    IEnumerator Delay() {

        yield return new WaitForSeconds(restartDelay);
        canRestart = true;
        if (restartButton != null) restartButton.SetActive(true);
    }

}

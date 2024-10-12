using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour {

    public TMP_Text scoreText;
    public int score;


    // Start is called before the first frame update
    void Start() {

        UpdateScoreText();
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddToScore(int points = 1) {

        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText() {

        if (scoreText != null) {

            scoreText.text = "SCORE: " + score.ToString();
        }
    }
}

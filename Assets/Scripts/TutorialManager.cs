using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour {

    public GameObject[] tutorialText;
    public UnityEvent tutorialFinished;
    public PlayerController playerController;
    private int index;
    public ScoreTracker scoreTracker;
    public GameObject[] tutorialAnimals;
    private int score;
    [SerializeField] float gapTime;
    [SerializeField] float text0Time;

    void Awake() {

        scoreTracker.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Start is called before the first frame update
    void Start() {

        index = 0;
        StartCoroutine("Tutorial");
    }

    // Update is called once per frame
    void Update() {

        score = scoreTracker.score;
    }

    IEnumerator Tutorial() {

        playerController.enableShooter = false;

        // Text 0
        tutorialText[index].SetActive(true);
        yield return new WaitForSeconds(text0Time);
        tutorialText[index].SetActive(false);
        yield return new WaitForSeconds(gapTime);
        index++;

        // Text 1
        tutorialText[index].SetActive(true);
        tutorialAnimals[0].SetActive(true);
        while (score < 30) {
            yield return null;
        }
        tutorialText[index].SetActive(false);
        yield return new WaitForSeconds(gapTime + 1);
        index++;

        // Text 2
        playerController.enableShooter = true;
        tutorialText[index].SetActive(true);
        tutorialAnimals[1].SetActive(true);
        while (score < 105) {
            yield return null;
        }
        tutorialText[index].SetActive(false);
        yield return new WaitForSeconds(gapTime);
        index++;

        // Text 3
        tutorialText[index].SetActive(true);
        tutorialAnimals[2].SetActive(true);
        yield return new WaitForSeconds(6);
        tutorialText[index].SetActive(false);
        yield return new WaitForSeconds(gapTime);
        index++;


        tutorialFinished.Invoke();
        Debug.Log("Tutorial done.");
        scoreTracker.AddToScore(-105);
        yield return new WaitForSeconds(3.0f);
        scoreTracker.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }
}

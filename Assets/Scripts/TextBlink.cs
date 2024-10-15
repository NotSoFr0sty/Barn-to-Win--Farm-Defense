using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlink : MonoBehaviour {

    private int numberOfBlinks = 4;
    private CanvasGroup canvasGroup;
    private float blinkDuration = 0.25f;
    public bool blinkEndsWithOff = false;

    // Start is called before the first frame update
    void Start() {

        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine("Blink");
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator Blink() {

        if (canvasGroup == null) yield break;

        if (blinkEndsWithOff) {
            for (int i = 0; i < numberOfBlinks; i++) {

                canvasGroup.alpha = 1;
                yield return new WaitForSeconds(blinkDuration);

                canvasGroup.alpha = 0;
                yield return new WaitForSeconds(blinkDuration);
            }
        }
        else {

            for (int i = 0; i < numberOfBlinks; i++) {

                canvasGroup.alpha = 0;
                yield return new WaitForSeconds(blinkDuration);

                canvasGroup.alpha = 1;
                yield return new WaitForSeconds(blinkDuration);
            }
        }
    }
}

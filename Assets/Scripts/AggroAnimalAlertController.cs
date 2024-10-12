using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroAnimalAlertController : MonoBehaviour {

    [SerializeField] private float xPositionOfAlert = 16.0f;
    public GameObject alertSprite;
    private GameObject alert;
    private Ballistics thisBallistics;
    [SerializeField] private float spriteDuration = 3.0f;

    // Start is called before the first frame update
    void Start() {

        thisBallistics = GetComponent<Ballistics>();
        spriteDuration = 20.0f / thisBallistics.speed;

        // If the aggroAnimal spawned on the left-hand side, then flip the sign of xPositionOfAlert.
        if (transform.position.x < 0) {
            xPositionOfAlert = -xPositionOfAlert;
        }

        //Spawn the alert
        StartCoroutine("spawnAlert");
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator spawnAlert() {

        // Instantiate the red exclamation mark (alert)
        alert = Instantiate(alertSprite, new Vector3(xPositionOfAlert, 5, transform.position.z), alertSprite.transform.rotation);

        yield return new WaitForSeconds(spriteDuration);

        // Destroy the alert
        Destroy(alert);
    }

    private void OnDestroy() {

        if (alert != null) {

            Destroy(alert);
        }
    }
}

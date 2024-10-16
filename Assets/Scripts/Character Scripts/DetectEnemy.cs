using UnityEngine;

public class DetectEnemy : MonoBehaviour {

    private HealthController enemyHealthController;
    private HealthController thisHealthController;

    // Start is called before the first frame update
    void Start() {

        thisHealthController = gameObject.GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter(Collision other) {

        // if character is invincible OR if character isBlinking, then ignore the rest
        if (thisHealthController.isInvincible || thisHealthController.isBlinking) {
            return;
        }

        if (other.gameObject.CompareTag("Enemy")) {

            // Take damage equal to the "damage" value of the enemy's healthController script    
            enemyHealthController = other.gameObject.GetComponent<HealthController>();
            thisHealthController.takeDamage(enemyHealthController.damage);
        }
    }


}

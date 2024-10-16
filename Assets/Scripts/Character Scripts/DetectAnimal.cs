using UnityEngine;

public class DetectAnimal : MonoBehaviour {
    private HealthController enemyHealthController;
    private HealthController thisHealthController;

    // Start is called before the first frame update
    void Start() {

        thisHealthController = gameObject.GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        // if character is invincible OR if character isBlinking, then ignore the rest
        if (thisHealthController.isInvincible) {
            return;
        }

        if (other.CompareTag("FarmAnimal") || other.CompareTag("Enemy")) {

            if (thisHealthController.isBlinking) {

                // Destroy the animal
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Enemy")) {

                if (other.TryGetComponent<MooseLogic>(out MooseLogic otherMooseLogic)) {

                    // Only allow turnt moose to deal damage to barn
                    if (otherMooseLogic.isTurning) {

                        // Take damage equal to the "damage" value of the enemy's healthController script    
                        enemyHealthController = other.gameObject.GetComponent<HealthController>();
                        thisHealthController.takeDamage(enemyHealthController.damage);

                        // Destroy the animal
                        Destroy(other.gameObject);
                    }
                    else {

                        // Destroy the animal
                        Destroy(other.gameObject);
                    }
                }
            }
            else {

                // Take damage equal to the "damage" value of the enemy's healthController script    
                enemyHealthController = other.gameObject.GetComponent<HealthController>();
                thisHealthController.takeDamage(enemyHealthController.damage);

                // Destroy the animal
                Destroy(other.gameObject);
            }

        }

    }

}

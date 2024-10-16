using UnityEngine;

public class DetectBullet : MonoBehaviour {

    private HealthController thisHealthController;
    private HealthController bulletHealthController;
    private HealthController playerHealthController;
    private PlayerController playerController;
    private Ballistics thisBallistics;
    [SerializeField][Range(0.0f, 1.0f)] private float speedChangeRatio = 0.5f;


    // Start is called before the first frame update
    void Start() {

        thisHealthController = gameObject.GetComponent<HealthController>();
        playerController = Component.FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        if (thisHealthController.isDead) {

            return;
        }

        // if character is invincible, then ignore the rest
        if (thisHealthController.isInvincible) {
            return;
        }

        thisBallistics = gameObject.GetComponent<Ballistics>();
        if (other.CompareTag("PlayerBullet")) {

            // Destroy the PlayerBullet
            Destroy(other.gameObject);

            // Reduce its speed
            thisBallistics.speed *= speedChangeRatio;

            // Damage this animal
            bulletHealthController = other.gameObject.GetComponent<HealthController>();
            thisHealthController.takeDamage(bulletHealthController.damage);
        }
        else if (other.CompareTag("PlayerMelee")) {

            // Destroy the last shot bullet
            Destroy(playerController.lastShotBullet);

            // Reduce its speed
            thisBallistics.speed *= speedChangeRatio;

            // Damage this animal
            playerHealthController = other.gameObject.transform.parent.gameObject.GetComponent<HealthController>();
            thisHealthController.takeDamage(playerHealthController.meleeDamage);

        }
    }

}

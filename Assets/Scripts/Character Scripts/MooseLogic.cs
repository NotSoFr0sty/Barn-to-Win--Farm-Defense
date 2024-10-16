using UnityEngine;

public class MooseLogic : MonoBehaviour {

    private bool hasTurned = false;
    public bool isTurning = false;
    private Ballistics thisBallistics;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float speedChangePercentage = 1.0f;
    private Rigidbody rb;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start() {

        rb = GetComponent<Rigidbody>();
        thisBallistics = GetComponent<Ballistics>();

        rotationSpeed = thisBallistics.speed / 2;

        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {

        if (isTurning) {

            rb.MoveRotation(Quaternion.Euler(0f, 180.0f, 0f));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 180.0f, 0f), Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other) {



        if (other.CompareTag("PlayerBullet")) {

            Destroy(other.gameObject);


            if (hasTurned) {
                return;
            }

            // Change the moose's speed (usually slower)
            thisBallistics.speed *= speedChangePercentage;
            rotationSpeed = thisBallistics.speed / 2;

            // Rotate moose to face bottom of screen
            isTurning = true;
            hasTurned = true;

            // Invoke moose warning text
            gameManager.StartCoroutine("WarnAboutMoose");
        }
    }
}

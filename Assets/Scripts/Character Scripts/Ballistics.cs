using UnityEngine;

public class Ballistics : MonoBehaviour {

    public float speed;
    public float originalSpeed;
    private Animator thisAnimator;
    [SerializeField] private float aliveTime = 15.0f;
    [SerializeField] private float xBounds = 23.0f;
    [SerializeField] private float zBounds = 40.0f;
    [SerializeField] private float yBounds = 40.0f;
    float difference;


    // Start is called before the first frame update
    void Start() {

        Destroy(gameObject, aliveTime);
        thisAnimator = GetComponent<Animator>();

        // difference = 10 - originalSpeed;
    }

    // Update is called once per frame
    void Update() {

        // Update speed_f
        if (thisAnimator != null) {

            thisAnimator.SetFloat("Speed_f", speed / originalSpeed);
        }

        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Destroy if out of bounds
        if (transform.position.x > xBounds || transform.position.x < -xBounds || transform.position.z > zBounds || transform.position.z < -5.0f || transform.position.y > yBounds || transform.position.y < -yBounds) {

            // // If the object is an animal, then say Game Over
            // if (!gameObject.CompareTag("PlayerBullet")) {

            //     Debug.Log("GAME OVER!");
            // }

            Destroy(gameObject);
        }


    }
}

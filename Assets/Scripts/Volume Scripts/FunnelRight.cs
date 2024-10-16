using UnityEngine;

public class FunnelRight : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 1.0f;
    GameObject animal;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (animal != null) {

            animal.transform.rotation = Quaternion.Lerp(animal.transform.rotation, Quaternion.Euler(0f, gameObject.transform.eulerAngles.y + 180, 0f), Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other) {

        animal = other.gameObject;
    }
}

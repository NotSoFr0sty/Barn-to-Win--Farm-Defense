using UnityEngine;

public class KillVoume : MonoBehaviour {

    [SerializeField] bool isBarn = false; // AKA shouldKillFarmAnimal
    public bool isSideGuards = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        if ((other.CompareTag("PlayerBullet") || other.CompareTag("Enemy")) && !isSideGuards) {

            // Destroy the PlayerBullet
            Destroy(other.gameObject);
        }

        if (isBarn && other.CompareTag("FarmAnimal")) {

            //Destroy the animal
            Destroy(other.gameObject);
        }
    }
}

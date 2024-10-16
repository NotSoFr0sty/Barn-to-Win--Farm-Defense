using UnityEngine;

public class AnimalSpawner : MonoBehaviour {

    private PlayerController player;
    public GameObject[] animalPrefabs;
    public float startDelay = 1.0f;
    public float spawnInterval = 1.0f;
    public bool isVerticalSpawner = false;
    public bool isShaped = false;
    public float speedModifier = 1.0f;

    // Start is called before the first frame update
    void Start() {

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        // invoke spawnAnimal every...
        Invoke("spawnAnimal", startDelay);
    }

    // Update is called once per frame
    void Update() {

        // // Spawn an animal when 'S' is pressed
        // if (Input.GetKeyDown(KeyCode.S)) {
        //     spawnAnimal();
        // }
    }

    void spawnAnimal() {
        /*
        Spawns a random animal within player.xRange
        */

        // Do not continue if it has been deactivated in hierarchy!!!
        if (!gameObject.activeInHierarchy) return;

        // Set animalIndex to a random integer from [0, 2]
        int animalIndex = Random.Range(0, animalPrefabs.Length);

        // Spawn
        if (isVerticalSpawner) {

            GameObject spawn = Instantiate(animalPrefabs[animalIndex], transform.position + new Vector3(0, 0, Random.Range(0.45f, player.zRange)), transform.rotation);
            spawn.GetComponent<Ballistics>().speed *= speedModifier;
        }
        else {

            GameObject spawn = Instantiate(animalPrefabs[animalIndex], transform.position + new Vector3(Random.Range(-player.xRange, player.xRange), 0, 0), transform.rotation);
            spawn.GetComponent<Ballistics>().speed *= speedModifier;
        }

        // invoke next spawnAnimal after some time
        if (isVerticalSpawner) {
            if (isShaped) {

                Invoke("spawnAnimal", Random.Range(spawnInterval, spawnInterval));
            }
            else Invoke("spawnAnimal", Random.Range(spawnInterval, spawnInterval + 5));
        }
        else {
            // ...every spawnInterval seconds
            Invoke("spawnAnimal", spawnInterval);
        }

    }


}

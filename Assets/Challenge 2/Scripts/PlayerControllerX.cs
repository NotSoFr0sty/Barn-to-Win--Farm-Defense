using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour {

    private bool isOnCooldown = false;
    private float cooldownDuration = 0.5f;

    public GameObject dogPrefab;

    // Update is called once per frame
    void Update() {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown) {

            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown() {

        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }
}

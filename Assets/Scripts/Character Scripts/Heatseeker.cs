using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatseeker : MonoBehaviour {

    private GameObject player;


    void Awake() {

        player = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
    }

    // Update is called once per frame
    void Update() {

    }
}

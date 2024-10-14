using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject lastShotBullet;
    public float speed;
    [SerializeField] private float movementSmoothing;
    [Range(0.0f, 18.0f)] public float xRange = 15.0f; // Maximum x-axis distance the player can travel from the origin
    [Range(0.0f, 18.0f)] public float zRange = 15.0f; // Mzximum z-axis distance the player can travel from the origin
    private float horizontalSmoothed = 0.0f;
    private float verticalSmoothed = 0.0f;
    private float elapsedTime = 0.0f;
    private CapsuleCollider proximityCollider;
    public GameObject meleeCollider;
    public GameObject meleeEffect;
    [SerializeField] private float meleeActiveTime;
    private bool isOnCooldown = false;
    [SerializeField] private float cooldownDuration;
    private bool canMelee = true;
    [SerializeField] private float fireRate = 2; // Maxiumum no. of bullets shot per second
    public bool enableShooter = true;
    public bool isFrozen = false;
    private Rigidbody rb;
    private float meleeEffectMinRadius = 0.23f;
    private float meleeEffectMaxRadius = 1.0f;
    public GameObject greenCircleOutline;
    Vector3 previousPosition = Vector3.zero;
    private Animator thisAnimator;

    // Start is called before the first frame update
    void Start() {

        proximityCollider = meleeCollider.GetComponent<CapsuleCollider>();
        proximityCollider.enabled = false;
        meleeEffect.SetActive(false);

        rb = GetComponent<Rigidbody>();

        if (greenCircleOutline != null) {

            meleeEffectMaxRadius = greenCircleOutline.transform.localScale.x;
        }

        thisAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() {

        if (isFrozen) {

            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            return;
        }

        // Move the player side-to-side based on horizontal input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        horizontalSmoothed = Mathf.Lerp(horizontalSmoothed, horizontalInput, movementSmoothing * Time.deltaTime);
        transform.Translate(Vector3.right * horizontalSmoothed * speed * Time.deltaTime, Space.World);

        // Constrict Player's x-axis position
        if (transform.position.x > xRange) {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xRange) {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        // Move the player forwards and backwards based on horizontal input
        float verticalInput = Input.GetAxisRaw("Vertical");
        verticalSmoothed = Mathf.Lerp(verticalSmoothed, verticalInput, movementSmoothing * Time.deltaTime);
        transform.Translate(Vector3.forward * verticalSmoothed * speed * Time.deltaTime, Space.World);

        // Constrict Player's z-axis position
        if (transform.position.z > zRange) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
        else if (transform.position.z < 0.45f) {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.45f);
        }

        // Press Spacebar to "shoot" food bullet
        if (Input.GetKey(KeyCode.Space)) {

            // Spawn a bullet
            //Instantiate(bulletPrefab, transform.position + new Vector3(0, 1.55f, 0.56f), bulletPrefab.transform.rotation);
            if (!isOnCooldown && enableShooter) {

                StartCoroutine("shootBullet");
            }


            // only allow melee "attacks" if canMelee is true
            if (canMelee) {

                // Activate proximityCollider for meleeActiveTime seconds
                StartCoroutine("melee");
            }
        }

        if (proximityCollider.enabled == true) {

            // the player is currently melee attacking, so play the melee effect animation
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / meleeActiveTime;
            float scale = Mathf.Lerp(meleeEffectMinRadius, meleeEffectMaxRadius, percentageComplete);
            meleeEffect.transform.localScale = new Vector3(scale, scale, scale);
        }

        // DEBUG VELOCITY
        Vector3 currentPosition = transform.position;
        Vector3 velocity = (currentPosition - previousPosition) / Time.deltaTime;
        // Debug.Log(velocity.magnitude);
        previousPosition = currentPosition;

        // Update speed_f
        if (thisAnimator != null) {

            thisAnimator.SetFloat("Speed_f", velocity.magnitude / 20);
            Debug.Log(velocity.magnitude / 20);
        }

    }

    IEnumerator melee() {
        /* Activates proximityCollider for meleeActiveTime seconds
        */

        // Melee start
        canMelee = false;
        meleeEffect.SetActive(true);
        proximityCollider.enabled = true;

        yield return new WaitForSeconds(meleeActiveTime);

        // Melee end
        meleeEffect.SetActive(false);
        elapsedTime = 0;
        proximityCollider.enabled = false;

        yield return new WaitForSeconds(cooldownDuration);
        canMelee = true;
    }

    IEnumerator shootBullet() {

        // Trigger throw animation
        thisAnimator.SetBool("isThrowing", true);

        // Enter cooldown
        isOnCooldown = true;

        // Spawn bullet
        lastShotBullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 1.55f, 0.56f), bulletPrefab.transform.rotation);

        yield return new WaitForSeconds(1 / fireRate);

        // Exit cooldown
        isOnCooldown = false;

        // Stop throw animation
        // yield return new WaitForSeconds(1);
        thisAnimator.SetBool("isThrowing", false);
    }

    public void FreezePlayer() {

        isFrozen = true;
    }

    public void UnfreezePlayer() {

        isFrozen = false;
    }
}

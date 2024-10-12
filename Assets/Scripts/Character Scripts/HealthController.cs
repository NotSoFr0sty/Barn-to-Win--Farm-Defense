using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    [SerializeField] private float health = 10, maxHealth = 10;
    public float damage = 1;
    public float meleeDamage = 1;
    public int points = 1;
    public bool isInvincible;
    public bool isBarn = false;
    private bool isTurning = false;
    [SerializeField] private float rotationSpeed = 1.0f;
    public bool isBallistic = true;
    public bool isBlinking = false; // Tracks gameObject "blinking" to indicate taking damage
    public SkinnedMeshRenderer blinkingMesh;
    public int numberOfBlinks = 4;
    public float blinkDurationOff = 0.25f;
    public float blinkDurationOn = 0.25f;
    public GameObject healthBar;
    public Vector3 healthBarOffset;
    private Vector3 minOffset;
    public float offsetRatio = 0.5f;
    public GameObject lastMealPrefab; // The piece of food that spawns when an animal "dies" (is full and eats).
    [SerializeField] private Vector3 lastMealOffset;
    [SerializeField] private float lastMealScaleFactor = 1;
    private Ballistics thisBallistics;
    private PlayerController thisPlayerController;
    private Rigidbody thisRigidbody;
    private Animator thisAnimator;
    public float deathDuration = 0.5f;
    public bool isDead = false;
    private ScoreTracker scoreTracker;
    public GameOverScreen gameOverScreen;

    // Start is called before the first frame update
    void Start() {

        if (isDead) {

            health = 0;
            StartCoroutine("die");
        }
        health = maxHealth;
        updateHealthBar();

        minOffset = new Vector3(healthBarOffset.x, healthBarOffset.y, healthBarOffset.z * offsetRatio);
    }

    // Update is called once per frame
    void Update() {

        if (isTurning) {

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 180.0f, 0f), Time.deltaTime * rotationSpeed);
        }

        if (healthBar != null && !isBarn) {

            healthBar.transform.rotation = Quaternion.Euler(90, 0, 0);


            float yRotInRadians = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            float angleValue = Mathf.Abs(Mathf.Cos(yRotInRadians));
            healthBar.transform.position = Vector3.Lerp(transform.position + minOffset, transform.position + healthBarOffset, angleValue);

            // healthBar.transform.position = transform.position + healthBarOffset;
        }
    }

    public void takeDamage(float damageAmount = 1) {

        // if the player is dead, then don't let it take damage
        if (isDead) {
            return;
        }

        health -= damageAmount;
        if (!isBlinking && !isBallistic) {

            StartCoroutine("takingDamageEffect");
        }
        else if (isBlinking && !isBallistic) {

            StopCoroutine("takingDamageEffect");
            if (blinkingMesh != null) {

                blinkingMesh.enabled = true;
            }
            StartCoroutine("takingDamageEffect");
        }

        if (health <= 0) {

            health = 0;
            StartCoroutine("die");
        }

        updateHealthBar();
    }

    void updateHealthBar() {
        if (isInvincible || healthBar == null) {
            return;
        }

        healthBar.GetComponent<Slider>().value = health / maxHealth;
    }

    public float getHealthPercentage() {

        return health / maxHealth;
    }

    IEnumerator die() {

        isDead = true;

        if (isBarn) {

            // Trigger the GameOver screen
            if (gameOverScreen != null) {

                gameOverScreen.StartGameOverScreen("BARN DESTROYED");
            }

            while (isBlinking) {
                yield return null;
            }

            // Deactivate healthbar
            if (healthBar != null) healthBar.SetActive(false);


            yield break;
        }

        thisAnimator = GetComponent<Animator>();
        if (isBallistic) {
            // If it's an animal...

            // Add to score
            scoreTracker = Component.FindObjectOfType<ScoreTracker>();
            if (scoreTracker != null) scoreTracker.AddToScore(points);

            // Deactivate healthbar
            if (healthBar != null) healthBar.SetActive(false);

            // Set speed to 0
            thisBallistics = GetComponent<Ballistics>();
            thisBallistics.speed = 0;

            // Freeze the animal's position and rotation
            thisRigidbody = GetComponent<Rigidbody>();
            thisRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            // Spawn a lastMeal in front of the animal
            if (lastMealPrefab != null) {

                GameObject lastMeal = Instantiate(lastMealPrefab, gameObject.transform.position, lastMealPrefab.transform.rotation);
                lastMeal.transform.SetParent(gameObject.transform, true);
                lastMeal.transform.localPosition += lastMealOffset;
                lastMeal.transform.localScale *= lastMealScaleFactor;
            }

            // Trigger the "eat" animation
            if (thisAnimator != null) {

                thisAnimator.SetTrigger("TrEat");
            }
            yield return new WaitForSeconds(deathDuration);
            Destroy(gameObject);

        }
        else {
            // else this is the Player

            // If the player's z position is < 5, then make the player face bottom of screen
            if (transform.position.z < 4) {

                isTurning = true;
            }

            // Indirectly freeze player position and rotation
            thisPlayerController = GetComponent<PlayerController>();
            thisPlayerController.isFrozen = true;

            // Play the death animation
            thisAnimator.SetTrigger("Tr_isDead");
            yield return new WaitForSeconds(deathDuration / 3);

            // Trigger the GameOver screen
            if (gameOverScreen != null) {

                gameOverScreen.StartGameOverScreen("YOU DIED");
            }

            // Destroy the player
            // yield return new WaitForSeconds(deathDuration / 2);
            // Destroy(gameObject);

        }

    }

    IEnumerator takingDamageEffect() {

        // if (blinkingMesh == null) {
        //     yield break;
        // }

        isBlinking = true;

        // blinkingMesh.enabled = false;
        // yield return new WaitForSeconds(blinkDurationOff);

        // blinkingMesh.enabled = true;
        // yield return new WaitForSeconds(blinkDurationOn);

        // blinkingMesh.enabled = false;
        // yield return new WaitForSeconds(blinkDurationOff);

        // blinkingMesh.enabled = true;
        // yield return new WaitForSeconds(blinkDurationOn);

        // blinkingMesh.enabled = false;
        // yield return new WaitForSeconds(blinkDurationOff);

        // blinkingMesh.enabled = true;
        // yield return new WaitForSeconds(blinkDurationOn);

        // blinkingMesh.enabled = false;
        // yield return new WaitForSeconds(blinkDurationOff);

        // blinkingMesh.enabled = true;
        // yield return new WaitForSeconds(blinkDurationOn);

        if (isBarn) {

            CanvasGroup canvasGroup = healthBar.GetComponent<CanvasGroup>();
            yield return new WaitForSeconds(blinkDurationOn);
            for (int i = 0; i < numberOfBlinks; i++) {

                if (canvasGroup != null) canvasGroup.alpha = 0;
                yield return new WaitForSeconds(blinkDurationOff);

                if (canvasGroup != null) canvasGroup.alpha = 1;
                yield return new WaitForSeconds(blinkDurationOff);
            }

        }
        else {

            for (int i = 0; i < numberOfBlinks; i++) {

                if (blinkingMesh != null) blinkingMesh.enabled = false;
                yield return new WaitForSeconds(blinkDurationOff);

                if (blinkingMesh != null) blinkingMesh.enabled = true;
                yield return new WaitForSeconds(blinkDurationOn);
            }
        }

        isBlinking = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowFire : MonoBehaviour {

    [SerializeField] bool isSmoke = false;
    public float minAlphaValue = 1.0f;
    public float maxAlphaValue = 1.0f;
    public float gameOverAlphaValue = 1.0f;
    [SerializeField] private float minScale = 2.0f;
    [SerializeField] private float maxScale = 4.0f;
    [SerializeField] private float gameOverScale = 6.0f;
    public GameObject barn;
    private HealthController barnHealthController;
    private SpriteRenderer spriteRenderer;
    private float healthPercentage;
    [SerializeField] float showFirePercentage = 0.50f; // Fire will be shown below this percentage

    // Start is called before the first frame update
    void Start() {

        if (barn != null) barnHealthController = barn.GetComponent<HealthController>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initial fire scale and visibility
        transform.localScale = minScale * Vector3.one;
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        if (isSmoke) {

            Color temp = spriteRenderer.color;
            temp.a = minAlphaValue;
            spriteRenderer.color = temp;
        }
    }

    // Update is called once per frame
    void Update() {

        if (barnHealthController == null) return;

        // Make the fire visible when healthPercentage <= showFirePercentage
        healthPercentage = barnHealthController.getHealthPercentage();
        if (healthPercentage <= showFirePercentage) {

            if (spriteRenderer != null) spriteRenderer.enabled = true;
        }
        healthPercentage /= showFirePercentage;

        // Set scale to gameOverScale when barn is destroyed
        if (healthPercentage <= 0) {

            if (isSmoke) {

                Color temp = spriteRenderer.color;
                temp.a = gameOverAlphaValue;
                spriteRenderer.color = temp;
            }

            transform.localScale = gameOverScale * Vector3.one;
            return;
        }

        if (isSmoke) {

            float alpha = Mathf.Lerp(maxAlphaValue, minAlphaValue, healthPercentage);
            Color temp = spriteRenderer.color;
            temp.a = alpha;
            spriteRenderer.color = temp;

        }

        // Interpolate between maxScale and minScale based on healthPercentage
        transform.localScale = Mathf.Lerp(maxScale, minScale, healthPercentage) * Vector3.one;

    }
}

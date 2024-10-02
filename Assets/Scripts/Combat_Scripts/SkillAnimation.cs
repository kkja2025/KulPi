using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAnimation : MonoBehaviour
{
    public Button moveButton;
    public Transform spriteToMove; // Assign the world-space sprite you want to move
    public float animationDuration = 1.0f; // How long the animation lasts
    public GameObject boss; // Assign the boss object here
    public Canvas canvas; // UI canvas where the button exists

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isAnimating = false;
    private float elapsedTime = 0f;

    private void Start()
    {
        spriteToMove.gameObject.SetActive(false); // Hide the sprite at start
        moveButton.onClick.AddListener(StartMoveAnimation); // Link button press to start animation
    }

    private void Update()
    {
        if (isAnimating)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration); // Normalized time between 0 and 1

            spriteToMove.position = Vector3.Lerp(startPos, targetPos, t); // Animate position using Lerp

            if (t >= 1.0f) // Animation finished
            {
                // Start the coroutine to wait 1 more second before hiding the sprite
                StartCoroutine(WaitBeforeHiding());
                isAnimating = false; // Stop further animation updates
            }
        }
    }

    public void StartMoveAnimation()
    {
        spriteToMove.gameObject.SetActive(true); // Show the sprite
        isAnimating = true;
        elapsedTime = 0f; // Reset elapsed time for new animation

        // Convert move button UI position to world space
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            moveButton.GetComponent<RectTransform>().position,
            Camera.main, // Assuming main camera is used
            out startPos
        );

        // Set target position to boss position in world space
        targetPos = boss.transform.position;
    }

    // Coroutine to wait for 1 second before hiding the sprite
    private IEnumerator WaitBeforeHiding()
    {
        yield return new WaitForSeconds(1.5f); // Wait for 1 second
        spriteToMove.gameObject.SetActive(false); // Hide the sprite after the delay
    }
}

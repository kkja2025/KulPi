using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    private Button button;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    public float scaleFactor = 1.1f; 
    public float animationDuration = 0.1f; 

    void Start()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        button.onClick.AddListener(AnimateButton);
    }

    void AnimateButton()
    {
        StartCoroutine(ScaleButton());
    }

    System.Collections.IEnumerator ScaleButton()
    {
        Vector3 enlargedScale = originalScale * scaleFactor;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, enlargedScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = enlargedScale;

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            rectTransform.localScale = Vector3.Lerp(enlargedScale, originalScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = originalScale;
    }
}

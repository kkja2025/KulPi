using UnityEngine;
using UnityEngine.UI;

public class InteractButtonPositioner : MonoBehaviour
{
    [SerializeField] private RectTransform buttonRectTransform; 
    [SerializeField] private Camera mainCamera;    
    private Transform targetObject;
    private Vector2 defaultPosition;

    private void Start()
    {
        // Store the default anchored position as a Vector2
        defaultPosition = buttonRectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (targetObject != null)
        {
            // Get the world position of the target object
            Vector3 worldPosition = targetObject.position;
            // Convert the world position to screen point
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPosition);
            
            // Directly set the button's anchored position to the screen point
            buttonRectTransform.position = screenPoint;
        }
        else
        {
            // Reset to default position when targetObject is null
            buttonRectTransform.anchoredPosition = defaultPosition;
        }
    }

    public void SetTargetObject(Transform target)
    {
        targetObject = target;
        if (target == null)
        {
            // Reset position if target is null
            buttonRectTransform.anchoredPosition = defaultPosition;
        }
    }
}

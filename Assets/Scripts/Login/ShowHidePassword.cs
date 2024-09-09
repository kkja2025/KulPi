using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this to use TextMeshPro

public class ShowHidePassword : MonoBehaviour
{
    public TMP_InputField passwordInputField; // TextMeshPro Input Field
    public Button showHideButton; // Button to toggle visibility
    public GameObject hideIcon; // "Hide" icon (closed eye)
    public GameObject showIcon; // "Show" icon (open eye)
    private bool isPasswordVisible = false;

    void Start()
    {
        // Ensure correct initial states
        hideIcon.SetActive(false); // Initially, hide the "Hide" icon (closed eye)
        showIcon.SetActive(true);  // Initially, show the "Show" icon (open eye)

        // Add listener for button clicks
        showHideButton.onClick.AddListener(TogglePasswordVisibility);

        // Start with password hidden
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate();
    }

    // Function to toggle password visibility and switch icons
    public void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            // Show the password and switch to "Hide" icon (closed eye)
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            hideIcon.SetActive(true);  // Show the "Hide" icon
            showIcon.SetActive(false); // Hide the "Show" icon
        }
        else
        {
            // Hide the password and switch to "Show" icon (open eye)
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            hideIcon.SetActive(false); // Hide the "Hide" icon
            showIcon.SetActive(true);  // Show the "Show" icon
        }

        // Refresh the input field
        passwordInputField.ForceLabelUpdate();
    }
}
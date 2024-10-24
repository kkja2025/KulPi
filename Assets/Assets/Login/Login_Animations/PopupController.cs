using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
     private Animator animator;
    public Button closeButton; 

    void Start()
    {
        animator = GetComponent<Animator>();
        closeButton.onClick.AddListener(HidePopup);
    }

    public void ShowPopup()
    {
        animator.Play("PopupAnimation");
    }

    public void HidePopup()
    {
        animator.Play("PopupHideAnimation");
    }
}

using System.Collections;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsVisible", false);  // Make sure it starts hidden
    }

    public void ShowPopup()
    {
        animator.SetBool("IsVisible", true);  // Trigger the show animation
    }

    public void HidePopup()
    {
        animator.SetBool("IsVisible", false);  // Trigger the hide animation
        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);  // Deactivate the GameObject after hiding
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ButtonPopEffect : MonoBehaviour
{
    public float popScale = 1.2f;
    public float animationDuration = 0.2f;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        transform.LeanScale(Vector3.one * popScale, animationDuration).setOnComplete(() => {
            transform.LeanScale(Vector3.one, animationDuration);
        });
    }
}
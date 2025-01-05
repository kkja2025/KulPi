using TMPro;
using UnityEngine;

public class SetAsteriskCharacter : MonoBehaviour
{
    public TMP_InputField passwordInputField;
    public char maskCharacter = '●';

    void Start()
    {
        if (passwordInputField != null)
        {
            passwordInputField.asteriskChar = maskCharacter;
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            passwordInputField.ForceLabelUpdate();
        }
    }
}

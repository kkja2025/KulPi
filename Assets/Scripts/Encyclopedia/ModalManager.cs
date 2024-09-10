using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    public GameObject modalPanel;

    public void ToggleModal()
    {
        modalPanel.SetActive(!modalPanel.activeSelf);
    }
    
    public void CloseModal()
    {
        modalPanel.SetActive(false);
    }
}

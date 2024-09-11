using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    public GameObject modalPanel;
    public GameObject[] encyclopediaPanels;

    
    

    public void ToggleModal()
    {
        modalPanel.SetActive(!modalPanel.activeSelf);
    }
    
    public void CloseModal()
    {
        modalPanel.SetActive(false);
    }

    public void ShowPanel(int buttonIndex)
    {
        
        for (int i=0; i < encyclopediaPanels.Length; i++)
        {
            encyclopediaPanels[i].SetActive(false);
        }

        if (buttonIndex >= 0 && buttonIndex < encyclopediaPanels.Length)
        {
            encyclopediaPanels[buttonIndex].SetActive(true);
        }

    }
}

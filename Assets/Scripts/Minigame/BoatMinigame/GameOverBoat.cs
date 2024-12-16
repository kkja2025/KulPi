using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverBoat : TutorialMenu
{ 

    protected override void StartGame()
    {
        BoatManager.Singleton.RestartAsync();
    }
}

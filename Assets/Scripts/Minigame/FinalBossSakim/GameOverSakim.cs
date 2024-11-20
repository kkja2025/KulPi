using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverSakim : TutorialMenu
{ 

    protected override void StartGame()
    {
        BattleManagerSakim.Singleton.RestartAsync();
    }
}

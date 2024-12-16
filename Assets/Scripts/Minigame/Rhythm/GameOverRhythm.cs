using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverRhythm : TutorialMenu
{ 

    protected override void StartGame()
    {
        RhythmManager.Singleton.RestartAsync();
    }
}

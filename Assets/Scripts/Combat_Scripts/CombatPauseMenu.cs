using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CombatPauseMenu : PauseMenu
{
    protected override void ReturnMainMenu()
    {
        BattleManager.Singleton.ExitBattle();
    }
}

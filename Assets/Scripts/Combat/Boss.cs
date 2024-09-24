using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health; 
    public Boss(int hp)
    {
        health = hp;
    }

    public void TakeUltimateDamage()
    {
        int damageAmount = 1;
        health -= damageAmount;

        Debug.Log("Boss health: " + health);
        if (health <= 0)
        {
            BattleManager.Singleton.Defeated();
        }
    }
}


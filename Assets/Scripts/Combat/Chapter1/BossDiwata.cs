using System;
using UnityEngine;
using TMPro;

public class BossDiwata : MonoBehaviour
{
    [SerializeField] public TMP_Text healthText;
    public int health; 
    public BossDiwata(int hp)
    {
        health = hp;
    }

    private void Start()
    {
        UpdateHealthDisplay();
    }

    public virtual void TakeUltimateDamage()
    {
        int damageAmount = 1;
        health -= damageAmount;
        UpdateHealthDisplay();
        Debug.Log("Boss health: " + health);
        if (health <= 0)
        {
            DiwataBattleManager.Singleton.Defeated();
        }
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {health}";
        }
    }
}
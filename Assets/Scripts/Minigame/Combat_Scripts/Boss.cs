using System;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private int health; 
    
    public Boss(int hp)
    {
        health = hp;
    }

    private void Start()
    {
        UpdateHealthDisplay();
    }

    public int GetHealth()
    {
        return health;
    }

    public virtual void TakeUltimateDamage()
    {
        int damageAmount = 1;
        health -= damageAmount;
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {health}";
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;
    public bool isPlayerTeam;

    [Header("stats")]
    public int attackPower;
    public int magicPower;
    public int speed;

    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int maxDEF;
    public int currentDEF;

    public bool hasActed;
    public bool isDead;

    public void Damaged(int dmage)
    {
        currentHP -= dmage;

        if (currentHP <= 0)
        {
            isDead = true;
            if (isDead == true)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Heal(int health)
    {
        if (currentHP > 0)
        {
            currentHP += health;
        }
    }

    public void DefBuff(int defBoost)
    {
        currentDEF += defBoost;
    }

    public void ResetTurn()
    {
        hasActed = false;
    }
}

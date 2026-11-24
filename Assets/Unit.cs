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

    // Combat values
    public float accuracy;

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
        // dmage variable makes uses of attacker's power
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

    public bool DodgeChance(int dodgeChance)
    {
        // subtract dodge chance value (speed) of target from attack chance which is 100 then add back
        accuracy = 1f - (dodgeChance / 100f);

        // Stops unit from being 100% untouchable
        accuracy = Mathf.Clamp(accuracy, 0.1f, 0.95f);

        if (accuracy < Random.Range(0f, 1f)) 
        {
            return true; // dodge 
        }

        return false; // Continue with hit
    }
}

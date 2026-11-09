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


    public bool hasActed;
    public bool isDead;

    public void Damaged(int dmage)
    {
        currentHP -= dmage;

        if (currentHP < 0)
        {
            currentHP = 0;
            isDead = true;
        }
    }

    public void ResetTurn()
    {
        hasActed = false;
    }
}

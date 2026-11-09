using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    public Unit actor; // Move performer
    public Unit target; 
    public string actionType; // Attack, item use, skill
    public int speedPriority; // Copy of actor's speed at turn start
}

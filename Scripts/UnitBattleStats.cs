using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleStats : MonoBehaviour
{

    public string playerName;
    public string monsterName;

    public float hpCurrent;
    public float hpMax;
    public float spCurrent;
    public float spMax;

    public float power;
    public float intelligence;
    public float skill;
    public float speed;
    public float defense;
    public float resistance;
    public float toughness;
    public float stamina;

    public float moveSpeed;
    public float hpRegen;
    public float spRegen;

    public string range1attack1;
    public string range1attack2;
    public string range1attack3;
    public string range1attack4;

    public string range2attack1;
    public string range2attack2;
    public string range2attack3;
    public string range2attack4;

    public string range3attack1;
    public string range3attack2;
    public string range3attack3;
    public string range3attack4;

    public string range4attack1;
    public string range4attack2;
    public string range4attack3;
    public string range4attack4;

    public List<string> movesKnown = new List<string>();

}

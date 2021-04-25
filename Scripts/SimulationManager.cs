using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public UnitBattleStats pStats;
    public UnitBattleStats eStats;
    public MovesDatabase db;


    public string SimulateAttack(string move, string whatToReturn, UnitBattleStats attacker, UnitBattleStats defender)
    {
        //core: (difference of stat / 50) * 5%
        float differenceMod = 5;
        float critChance = 0;
        float hitChance = 0;
        float spCost = 0;
        float cooldown = 0;
        float hpDamage = 0;
        float spDamage = 0;

        foreach (var item in db.allMovesList)
        {
            if(item.moveName == move)
            {
                //damage calculations
                if (item.type == "Power")
                {
                    var powDifference = attacker.power - defender.defense;
                    if (powDifference <= 0)
                    {
                        powDifference = 0;
                    }
                    hpDamage = item.hpDamage + (item.hpDamage * ((powDifference / 50 * differenceMod) /100));
                    spDamage = item.spDamage + (item.spDamage * ((powDifference / 50 * differenceMod) / 100));
                }
                else if (item.type == "Intelligence")
                {
                    var intDifference = attacker.intelligence - defender.resistance;
                    if (intDifference <= 0)
                    {
                        intDifference = 0;
                    }
                    hpDamage = item.hpDamage + (item.hpDamage * ((intDifference / 50 * differenceMod) / 100));
                    spDamage = item.spDamage + (item.spDamage * ((intDifference / 50 * differenceMod) / 100));
                }

                // hit chance calculations
                var skiDifference = attacker.skill - defender.speed;
                if (skiDifference <= 0)
                {
                    skiDifference = 0;
                }
                hitChance = item.hitChance + (item.hitChance * ((skiDifference / 50 * differenceMod) / 100));

                //others
                spCost = item.spCost;
                cooldown = item.cooldown;
                critChance = item.critChance;
            }
        }


        var returnString = "";
        switch(whatToReturn)
        {
            case "hitChance":
                returnString = Mathf.RoundToInt(hitChance) + "";
                break;
            case "spCost":
                returnString = Mathf.RoundToInt(spCost) + "";
                break;
            case "cooldown":
                returnString = Mathf.RoundToInt(cooldown) + "";
                break;
            case "hpDamage":
                returnString = Mathf.RoundToInt(hpDamage) + "";
                break;
            case "spDamage":
                returnString = Mathf.RoundToInt(spDamage) + "";
                break;
            case "critChance":
                returnString = Mathf.RoundToInt(critChance) + "";
                break;
        }
        return returnString;

    }
}

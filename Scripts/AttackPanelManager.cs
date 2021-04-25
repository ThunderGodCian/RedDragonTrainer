using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPanelManager : MonoBehaviour
{
    public UnitBattleStats pStats;
    public UnitBattleStats eStats;
    public BattleManager battleManager;
    public SimulationManager simulationManager;
    public Text battleRange;

    public Button attack1;
    public Text a1Name;
    public Text a1SpCost;
    public Text a1HitChance;

    public Button attack2;
    public Text a2Name;
    public Text a2SpCost;
    public Text a2HitChance;

    public Button attack3;
    public Text a3Name;
    public Text a3SpCost;
    public Text a3HitChance;

    public Button attack4;
    public Text a4Name;
    public Text a4SpCost;
    public Text a4HitChance;

    public List<string> cooldownList = new List<string>();

    private void Start()
    {
        attack1.onClick.AddListener( () => Attack1Clicked(a1Name.text));
        attack2.onClick.AddListener( () => Attack1Clicked(a2Name.text));
        attack3.onClick.AddListener( () => Attack1Clicked(a3Name.text));
        attack4.onClick.AddListener( () => Attack1Clicked(a4Name.text));

        attack1.gameObject.SetActive(false);
        attack2.gameObject.SetActive(false);
        attack3.gameObject.SetActive(false);
        attack4.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(battleManager.battleRange >= 0 && battleManager.battleRange < 2)
        {
            a1Name.text = pStats.range1attack1;
            a2Name.text = pStats.range1attack2;
            a3Name.text = pStats.range1attack3;
            a4Name.text = pStats.range1attack4;
            battleRange.text = "Close Range";
        }
        else if (battleManager.battleRange >= 2 && battleManager.battleRange < 4)
        {
            a1Name.text = pStats.range2attack1;
            a2Name.text = pStats.range2attack2;
            a3Name.text = pStats.range2attack3;
            a4Name.text = pStats.range2attack4;
            battleRange.text = "Mid Range";
        }
        else if (battleManager.battleRange >= 4 && battleManager.battleRange <= 6)
        {
            a1Name.text = pStats.range3attack1;
            a2Name.text = pStats.range3attack2;
            a3Name.text = pStats.range3attack3;
            a4Name.text = pStats.range3attack4;
            battleRange.text = "Long Range";
        }
        else if (battleManager.battleRange >= 6 && battleManager.battleRange <= 8)
        {
            a1Name.text = pStats.range4attack1;
            a2Name.text = pStats.range4attack2;
            a3Name.text = pStats.range4attack3;
            a4Name.text = pStats.range4attack4;
            battleRange.text = "Extreme Range";
        }

        //disable if  null
        if (a1Name.text != "")
        {
            attack1.gameObject.SetActive(true);
            a1SpCost.text = CheckForSimulation(a1Name.text, "spCost") + "SP";
            a1HitChance.text = CheckForSimulation(a1Name.text, "hitChance")  + "%";
        }
        else
        {
            attack1.gameObject.SetActive(false);
        }

        if (a2Name.text != "")
        {
            attack2.gameObject.SetActive(true);
            a2SpCost.text = CheckForSimulation(a2Name.text, "spCost") + "SP";
            a2HitChance.text = CheckForSimulation(a2Name.text, "hitChance") + "%";
        }
        else
        {
            attack2.gameObject.SetActive(false);
        }

        if (a3Name.text != "")
        {
            attack3.gameObject.SetActive(true);
            a3SpCost.text = CheckForSimulation(a3Name.text, "spCost") + "SP";
            a3HitChance.text = CheckForSimulation(a3Name.text, "hitChance") + "%";
        }
        else
        {
            attack3.gameObject.SetActive(false);
        }

        if (a4Name.text != "")
        {
            attack4.gameObject.SetActive(true);
            a4SpCost.text = CheckForSimulation(a4Name.text, "spCost") + "SP";
            a4HitChance.text = CheckForSimulation(a4Name.text, "hitChance") + "%";
        }
        else
        {
            attack4.gameObject.SetActive(false);
        }

    }

    public void Attack1Clicked(string moveName)
    {
        battleManager.MoveTriggered(moveName, pStats, eStats);
    }
    public void Attack2Clicked(string moveName)
    {
        battleManager.MoveTriggered(moveName, pStats, eStats);
    }
    public void Attack3Clicked(string moveName)
    {
        battleManager.MoveTriggered(moveName, pStats, eStats);
    }
    public void Attack4Clicked(string moveName)
    {
        battleManager.MoveTriggered(moveName, pStats, eStats);
    }

    //keyboard control presses
    public void Attack1Pressed()
    {
        battleManager.MoveTriggered(a1Name.text, pStats, eStats);
    }
    public void Attack2Pressed()
    {
        battleManager.MoveTriggered(a2Name.text, pStats, eStats);
    }
    public void Attack3Pressed()
    {
        battleManager.MoveTriggered(a3Name.text, pStats, eStats);
    }
    public void Attack4Pressed()
    {
        battleManager.MoveTriggered(a4Name.text, pStats, eStats);
    }

    public string CheckForSimulation(string move, string whatToReturn)
    {
        var returnString = simulationManager.SimulateAttack(move, whatToReturn, pStats, eStats);
        return returnString;
    }

    public bool CheckForCooldown(string move)
    {
        if(cooldownList.Count > 0)
        {
            foreach (var item in cooldownList)
            {
                if (move == item)
                {
                    return true;
                }
            }
            cooldownList.Add(move);
            return false;
        }
        else
        {
            cooldownList.Add(move);
            return false;
        }
    }

    IEnumerator CooldownLocked(string move)
    {
        var tempCd = simulationManager.SimulateAttack(move, "cooldown", pStats, eStats);
        var cd = int.Parse(tempCd);
        yield return new WaitForSeconds(cd);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    #region singleton
    //singleton
    private static PlayerStats _instance;
    public static PlayerStats Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerStats>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    //end singleton
    #endregion singleton

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

    public float technique;

    public float tiredness;
    public float stress;

    public float moveSpeed;
    public float hpRegen;
    public float mpRegen;

    public int week;
    public int month;
    public int year;
    
    public int ageYear;
    public int ageMonth;
    public int ageWeek;

    public float funds;
    public float worldHp;

    public int trainingLevel;
    public int restLevel;
    public int earthLevel;
    public int fundsLevel;

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

    public List<string> activityKnown = new List<string>();
    public List<string> trainingKnown = new List<string>();
    public List<string> battleSkillsKnown = new List<string>();

    public int trainingIncrement;
    public string difficulty;

    private void Update()
    {
        if(week > 4)
        {
            month++;
            week = 0;
        }

        if (month > 12)
        {
            year++;
            month = 0;
        }

        if (ageWeek > 4)
        {
            ageMonth++;
            ageWeek = 0;
        }

        if (ageMonth > 12)
        {
            ageYear++;
            ageMonth = 0;
        }
    }

    public int TrainStat(string stat)
    {
        var trainingIncrementTiredness = 0;
        //tiredness
        if (PlayerStats.Instance.tiredness <= 25)
        {
            trainingIncrementTiredness = Random.Range(8, 10);
        }
        else if (PlayerStats.Instance.tiredness > 25 && PlayerStats.Instance.tiredness <= 50)
        {
            trainingIncrementTiredness = Random.Range(7, 9);
        }
        else if (PlayerStats.Instance.tiredness > 50 && PlayerStats.Instance.tiredness <= 75)
        {
            trainingIncrementTiredness = Random.Range(3, 5);
        }
        else if (PlayerStats.Instance.tiredness > 75)
        {
            trainingIncrementTiredness = Random.Range(1, 2);
        }

        //stress
        var trainingIncrementStress = 0;
        if (PlayerStats.Instance.stress <= 25)
        {
            trainingIncrementStress = Random.Range(8, 10);
        }
        else if (PlayerStats.Instance.stress > 25 && PlayerStats.Instance.stress <= 50)
        {
            trainingIncrementStress = Random.Range(7, 9);
        }
        else if (PlayerStats.Instance.stress > 50 && PlayerStats.Instance.stress <= 75)
        {
            trainingIncrementStress = Random.Range(3, 5);
        }
        else if (PlayerStats.Instance.stress > 75)
        {
            trainingIncrementStress = Random.Range(1, 2);
        }
        //total increment
        trainingIncrement = trainingIncrementTiredness + trainingIncrementStress;

        foreach (var property in this.GetType().GetFields())
        {
            //Debug.Log("Name: " + property.Name + " Value: " + property.GetValue(this));
            if (stat.ToLower() == property.Name.ToLower())
            {
                var tempStat = int.Parse(property.GetValue(this).ToString());
                property.SetValue(this, tempStat + trainingIncrement);
            }
        }
        tiredness += 15;
        stress += 12;
        return trainingIncrement;
    }

    public void Rest()
    {
        tiredness -= 33;
        stress -= 5;
        tiredness = Mathf.Clamp(tiredness, 0, 100);
        stress = Mathf.Clamp(stress, 0, 100);
    }

    public void UpgradeStat(string stat)
    {
        var levelIncrement = 1;
        foreach (var property in this.GetType().GetFields())
        {
            if (stat.ToLower() == property.Name.ToLower())
            {
                var tempStat = int.Parse(property.GetValue(this).ToString());
                property.SetValue(this, tempStat + levelIncrement);
            }
        }
    }

    public int CostToUpgrade(string stat)
    {
        var cost = 0;
        foreach (var property in this.GetType().GetFields())
        {
            if (stat.ToLower() == property.Name.ToLower())
            {
                var upgradeLevel = int.Parse(property.GetValue(this).ToString());
                cost = upgradeLevel * 5000;
            }
        }
        return cost;
    }

    public string ReturnStatValue(string stat)
    {
        var statValue = "";
        foreach (var property in this.GetType().GetFields())
        {
            if (stat.ToLower() == property.Name.ToLower())
            {
                //print(stat.ToLower());
                //print(property.Name.ToLower());
                var temp = property.GetValue(this).ToString();
                statValue = temp;
                //statValue = int.Parse(property.GetValue(this).ToString());
            }
            
        }
        return statValue;
    }
}

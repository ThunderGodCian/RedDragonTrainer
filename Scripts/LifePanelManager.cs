using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanelManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Text hpText;
    public Slider hpSlider;
    public Text spText;
    public Slider spSlider;
    public Text nameText;
    public UnitBattleStats unitStats;

    [Header("Logic")]
    public int maxHp;
    public int Hp;
    public int maxSp;
    public int Sp;

    private void Start()
    {
        
    }

    private void Update()
    {
        hpText.text = Mathf.RoundToInt(unitStats.hpCurrent) + "/" + Mathf.RoundToInt(unitStats.hpMax);
        hpSlider.value = (unitStats.hpCurrent / unitStats.hpMax);
        spText.text = Mathf.RoundToInt(unitStats.spCurrent) + "/" + Mathf.RoundToInt(unitStats.spMax);
        spSlider.value = (unitStats.spCurrent / unitStats.spMax);
        nameText.text = unitStats.monsterName;
    }
}

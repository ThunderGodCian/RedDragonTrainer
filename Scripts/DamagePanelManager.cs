using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamagePanelManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public BattleManager battleManager;
    public Text hpDamage;
    public Text spDamage;
    public Text moveName;
    public Text hitChance;

    public void ShowDamage(int hp, int sp, int hit, string move)
    {
        this.gameObject.SetActive(true);

        if(battleManager.animHitCheck)
        {
            StartCoroutine(ShowDamageOnScreen(hp, sp, hit, move));
        }
        else if (!battleManager.animHitCheck) 
        {
            StartCoroutine(ShowDamageOnScreen("", hit, move));
        }
    }

    public void ShowMoveDuringPause(string move)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowMoveName(move));
    }

    IEnumerator ShowDamageOnScreen(int hp, int sp, int hit, string move)
    {
        moveName.text = move;
        hpDamage.text = hp + " Damage";
        spDamage.text = "SP -" + sp;
        yield return new WaitForSeconds(4f);
        hpDamage.text = "";
        spDamage.text = "";
        moveName.text = "";
        this.gameObject.SetActive(false);
    }

    IEnumerator ShowDamageOnScreen(string missed, int hit, string move)
    {
        moveName.text = move;
        hitChance.text = "Chance to Hit: " + hit + "%";
        hpDamage.text = "Miss! ";
        spDamage.text = "";
        yield return new WaitForSeconds(4f);
        hpDamage.text = "";
        spDamage.text = "";
        moveName.text = "";
        this.gameObject.SetActive(false);
    }

    IEnumerator ShowMoveName(string move)
    {
        moveName.text = move;
        hitChance.text = "";
        hpDamage.text = "";
        spDamage.text = "";
        yield return new WaitForSeconds(4f);
        moveName.text = "";
        hitChance.text = "";
        hpDamage.text = "";
        spDamage.text = "";
    }

    IEnumerator ShowHpDamage(int hp, int sp)
    {
        hpDamage.text = hp + " Damage";
        spDamage.text = "SP -" + sp;
        yield return new WaitForSeconds(4f);
        moveName.text = "";
        hitChance.text = "";
        hpDamage.text = "";
        spDamage.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public UnitBattleStats unitStats;
    public BattleManager battleManager;
    public AttackPanelManager attackPanel;
    public Animator animator;
    public GameObject attackVFX;
    public GameObject attackVFX2;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public float volume = 1f;

    public float comAttackCooldown;
    public bool isComAction = false;
    public bool isComOn = true;

    private void Start()
    {

    }

    void Update()
    {
        comAttackCooldown = Random.Range(0, 10);

        unitStats.hpCurrent = Mathf.Clamp(unitStats.hpCurrent, 0, 999);
        unitStats.spCurrent = Mathf.Clamp(unitStats.spCurrent, 0, 999);

        if (battleManager.battleState != BattleManager.BATTLESTATE.READY)
        {
            return;
        }

        if(isComOn)
        {
            if (unitStats.playerName == "COM")
            {
                MoveCom(Random.Range(1, 4));
            }

            if (unitStats.playerName == "COM")
            {
                if (isComAction)
                {
                    return;
                }
                StartCoroutine(WaitForSecondsCom());
                isComAction = true;
            }
        }



        if (unitStats.playerName != "COM")
        {
            if (Input.GetKey(KeyCode.D))
            {
                MoveForward();
            }
            if (Input.GetKey(KeyCode.A))
            {
                MoveBackward();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                attackPanel.Attack1Pressed();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                attackPanel.Attack2Pressed();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                attackPanel.Attack3Pressed();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                attackPanel.Attack4Pressed();
            }
        }


    }


    IEnumerator WaitForSecondsCom()
    {
        RandomAttackCom();
        yield return new WaitForSeconds(comAttackCooldown);
        isComAction = false;

    }
    public void RandomAttackCom()
    {
        if(unitStats.hpCurrent <= 0)
        {
            return;
        }
        if (battleManager.battleState == BattleManager.BATTLESTATE.READY)
        {
            var i = Random.Range(0, unitStats.movesKnown.Count);
            battleManager.MoveTriggered(unitStats.movesKnown[i], battleManager.estats, battleManager.pstats);
        }
    }

    public void MoveCom(float preferredRange)
    {
        if(battleManager.battleState == BattleManager.BATTLESTATE.READY)
        {
            if (battleManager.battleRange > preferredRange)
            {
                battleManager.WalkForwardCom(unitStats);
                //this.animator.SetTrigger("Walk");
            }
        }

    }


    public void MoveForward()
    {
        if (battleManager.battleState == BattleManager.BATTLESTATE.READY)
            //this.transform.Translate(Vector3.forward * Time.deltaTime * 10);
            battleManager.WalkForward(unitStats);
            //this.animator.SetTrigger("Walk");
    }

    public void MoveBackward()
    {
        if (battleManager.battleState == BattleManager.BATTLESTATE.READY)
            //this.transform.Translate(Vector3.back * Time.deltaTime * 10);
            battleManager.WalkBackward(unitStats);
            //this.animator.SetTrigger("Walk");
    }

    public void DashForward()
    {
        battleManager.DashForward(unitStats);
    }

    public void DashBackward()
    {
        battleManager.DashBackward(unitStats);
    }

    public void HitEvent()
    {
        if (battleManager.animHitCheck)
        {
            print(unitStats.monsterName + " HIT " + battleManager.animDefender);
            audioSource.PlayOneShot(hitSound, volume);
            battleManager.animDefender.SetTrigger("Hit");
            var effect = Instantiate(attackVFX2, battleManager.animDefender.gameObject.transform);
            Destroy(effect, 1f);
        }
        else if(!battleManager.animHitCheck)
        {
            print(unitStats.monsterName + " MISS " + battleManager.animDefender);
            battleManager.animDefender.SetTrigger("Backflip");
        }
    }

    public void FinishedEvent()
    {
        battleManager.isAnimPlaying = false;
        battleManager.isActionOngoing = false;
    }

    public void TakeDamage(float hpDamage, float spDamage)
    {
        gameObject.GetComponent<UnitBattleStats>().hpCurrent -= hpDamage;
        gameObject.GetComponent<UnitBattleStats>().spCurrent -= spDamage;
    }

}

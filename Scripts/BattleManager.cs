using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public enum BATTLESTATE
    {
        INIT,
        OPENING,
        READY,
        ACTIONSTART,
        COUNTERWINDOW,
        ANIMATE,
        END
    }
    public BATTLESTATE battleState;

    [Header("Set in Inspector")]
    public GameObject bannerPanel;
    public GameObject topPanel;
    public GameObject bottomPanel;
    public GameObject damagePanel;
    public UnitController playerController;
    public SimulationManager simulationManager;
    public UnitBattleStats pstats;
    public UnitBattleStats estats;
    public CinemachineVirtualCamera vCamBoth;
    public CinemachineVirtualCamera vCamP1;
    public CinemachineVirtualCamera vCamP2;
    public GameObject pArmature;
    public GameObject eArmature;

    public AudioSource audioSource;
    public AudioClip battleTheme;
    public AudioClip moveStartSound;
    public AudioClip winSound;
    public float volume = 1f;

    [Header("Logic")]
    public float minX = -40f;
    public float maxX = 40f;
    public float distanceBetween;
    public bool isActionOngoing = false;
    public bool isAnimPlaying = false;
    public float battleRange;
    public Animator animAttacker;
    public Animator animDefender;
    public bool animHitCheck = false;
    public bool isDead = false;
    public bool isCounterClockOn = false;
    public bool isPauseBeforeAttack = false;

    private void Start()
    {
        audioSource.clip = battleTheme;
        audioSource.loop = true;
        audioSource.Play();

        battleState = BATTLESTATE.INIT;
        CalculateDistanceBetween();

        LoadStatsFromPlayerStats();
        SetEnemyStats();
    }



    private void Update()
    {

        switch (battleState)
        {
            case BATTLESTATE.INIT:
                bannerPanel.SetActive(false);
                topPanel.SetActive(false);
                bottomPanel.SetActive(false);
                damagePanel.SetActive(false);

                vCamBoth.gameObject.SetActive(false);
                vCamP1.gameObject.SetActive(false);
                vCamP2.gameObject.SetActive(false);

                battleState = BATTLESTATE.OPENING;
                break;

            case BATTLESTATE.OPENING:
                if(isActionOngoing)
                {
                    return;
                }
                isActionOngoing = true;
                bannerPanel.SetActive(true);
                topPanel.SetActive(true);
                bottomPanel.SetActive(true);
                
                vCamBoth.gameObject.SetActive(true);

                StartCoroutine(CountdownTimer(3.9f));

                break;

            case BATTLESTATE.READY:
                bannerPanel.SetActive(true);
                topPanel.SetActive(true);
                bottomPanel.SetActive(true);

                vCamBoth.gameObject.SetActive(true);
                vCamP1.gameObject.SetActive(false);
                vCamP2.gameObject.SetActive(false);

                CheckIfDead();
                CalculateDistanceBetween();
                FaceOpponent();
                SpRegen();
                break;

            case BATTLESTATE.ANIMATE:
                FaceOpponent();

                if (!isAnimPlaying)
                {
                    battleState = BATTLESTATE.READY;
                }
                break;

            case BATTLESTATE.END:
                if (isActionOngoing)
                {
                    return;
                }
                isActionOngoing = true;
                
                break;
        }
    }

    IEnumerator WaitBeforeLoading()
    {
        yield return new WaitForSeconds(3f);
        SceneLoader.Instance.LoadScene("Home");
    }

    public void CheckIfDead()
    {
        if(pstats.hpCurrent <= 0)
        {
            //dead
            battleState = BATTLESTATE.END;
            audioSource.PlayOneShot(winSound, volume);
            StartCoroutine(ShowMessageOnBanner("You Lose", 999));
            StartCoroutine(WaitBeforeLoading());
            PlayerStats.Instance.stress += 75;
            PlayerStats.Instance.tiredness += 75;
            if (isActionOngoing)
            {
                return;
            }
            isActionOngoing = true;
            pstats.gameObject.GetComponent<Animator>().SetTrigger("Dead");
            
        }
        else if (estats.hpCurrent <= 0)
        {
            //enemy dead.
            audioSource.PlayOneShot(winSound, volume);
            battleState = BATTLESTATE.END;
            StartCoroutine(ShowMessageOnBanner("You Win", 999));
            StartCoroutine(WaitBeforeLoading());
            if (isActionOngoing)
            {
                return;
            }
            isActionOngoing = true;
            estats.gameObject.GetComponent<Animator>().SetTrigger("Dead");
        }

    }

    public void WalkForward(UnitBattleStats unit)
    {
        if(Mathf.Abs(battleRange) < 1f)
        {
            return;
        }

        unit.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * unit.moveSpeed);
        if(unit.gameObject.transform.position.x < minX)
        {
            unit.gameObject.transform.position = new Vector3(minX,0,0);
            pstats.gameObject.GetComponent<Animator>().SetTrigger("Walk");
        }
        if (unit.gameObject.transform.position.x > maxX)
        {
            unit.gameObject.transform.position = new Vector3(maxX, 0, 0);
        }
    }

    public void WalkBackward(UnitBattleStats unit)
    {
        unit.gameObject.transform.Translate(Vector3.back * Time.deltaTime * unit.moveSpeed);
        if (unit.gameObject.transform.position.x < minX)
        {
            unit.gameObject.transform.position = new Vector3(minX, 0, 0);
        }
        if (unit.gameObject.transform.position.x > maxX)
        {
            unit.gameObject.transform.position = new Vector3(maxX, 0, 0);
        }
    }

    public void WalkForwardCom(UnitBattleStats unit)
    {
        if (Mathf.Abs(battleRange) < 1f)
        {
            return;
        }

        unit.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * unit.moveSpeed);
        if (unit.gameObject.transform.position.x < minX)
        {
            unit.gameObject.transform.position = new Vector3(minX, 0, 0);
        }
        if (unit.gameObject.transform.position.x > maxX)
        {
            unit.gameObject.transform.position = new Vector3(maxX, 0, 0);
        }
    }
    public void WalkBackwardCom(UnitBattleStats unit)
    {
        unit.gameObject.transform.Translate(Vector3.back * Time.deltaTime * unit.moveSpeed);
        if (unit.gameObject.transform.position.x < minX)
        {
            unit.gameObject.transform.position = new Vector3(minX, 0, 0);
        }
        if (unit.gameObject.transform.position.x > maxX)
        {
            unit.gameObject.transform.position = new Vector3(maxX, 0, 0);
        }
    }


    IEnumerator CountdownTimer(float seconds)
    {
        var bannerField = GameObject.Find("BannerPanelMessageText").GetComponent<Text>();
        var countdown = seconds;
        while((Mathf.RoundToInt(countdown)) > 0)
        {
            countdown -= Time.deltaTime;
            bannerField.text = (Mathf.RoundToInt(countdown)).ToString();
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ShowMessageOnBanner("Fight!", 2f));
        
        battleState = BATTLESTATE.READY;
        isActionOngoing = false;
    }

    IEnumerator ShowMessageOnBanner(string message, float seconds)
    {
        var bannerField = GameObject.Find("BannerPanelMessageText").GetComponent<Text>();
        bannerField.text = message;
        yield return new WaitForSeconds(seconds);
        bannerField.text = "";
    }

    public void MoveTriggered(string move, UnitBattleStats atkStats, UnitBattleStats defStats)
    {
        if (battleState == BATTLESTATE.READY && !isAnimPlaying)
        {
            var spCostX = simulationManager.SimulateAttack(move, "spCost", atkStats, defStats);
            int spCost = int.Parse(spCostX);
            if (spCost < atkStats.spCurrent)
            {
                StartCoroutine(PauseBeforeAttacking(move, atkStats, defStats));

                isAnimPlaying = true;
                battleState = BATTLESTATE.ANIMATE;
            }
        }
    }

    IEnumerator PauseBeforeAttacking(string move, UnitBattleStats atkStats, UnitBattleStats defStats)
    {
        var spCostX = simulationManager.SimulateAttack(move, "spCost", atkStats, defStats);
        int spCost = int.Parse(spCostX);
        if (spCost < atkStats.spCurrent)
        {
            isPauseBeforeAttack = true;
            if (atkStats.playerName == "COM")
            {
                vCamP2.gameObject.SetActive(true);
            }
            else
            {
                vCamP1.gameObject.SetActive(true);
            }
            damagePanel.GetComponent<DamagePanelManager>().ShowMoveDuringPause(move);

            yield return new WaitForSeconds(1f);
            isPauseBeforeAttack = false;
            ExecuteMove(move, atkStats, defStats);
        }


    }

    public void ExecuteMove(string move, UnitBattleStats atkStats, UnitBattleStats defStats)
    {


        vCamBoth.gameObject.SetActive(true);
        vCamP1.gameObject.SetActive(false);
        vCamP1.gameObject.SetActive(false);

        var hpDamageX = simulationManager.SimulateAttack(move, "hpDamage", atkStats, defStats);
        var spDamageX = simulationManager.SimulateAttack(move, "spDamage", atkStats, defStats);
        var spCostX = simulationManager.SimulateAttack(move, "spCost", atkStats, defStats);
        var hitChanceX = simulationManager.SimulateAttack(move, "hitChance", atkStats, defStats);
        var critChanceX = simulationManager.SimulateAttack(move, "critChance", atkStats, defStats);

        int hpDamage = int.Parse(hpDamageX);
        int spDamage = int.Parse(spDamageX);
        int spCost = int.Parse(spCostX);
        int hitChance = int.Parse(hitChanceX);
        int critChance = int.Parse(critChanceX);

        print(spCost);
        print(atkStats.spCurrent);

        if (spCost < atkStats.spCurrent)
        {
            audioSource.PlayOneShot(moveStartSound, volume);

            animAttacker = atkStats.gameObject.GetComponent<Animator>();
            animDefender = defStats.gameObject.GetComponent<Animator>();

            //subtract SpCost
            atkStats.spCurrent -= spCost;
            //rng
            int rollForHit = Random.Range(0, 100);
            //check if hit
            if (hitChance >= rollForHit)
            {
                animHitCheck = true;
                //print("HIT!! " + hitChance + " vs " + rollForHit);
                atkStats.gameObject.GetComponent<Animator>().SetTrigger(move);
                defStats.gameObject.GetComponent<UnitBattleStats>().hpCurrent -= hpDamage;
                defStats.gameObject.GetComponent<UnitBattleStats>().spCurrent -= spDamage;
                damagePanel.GetComponent<DamagePanelManager>().ShowDamage(hpDamage, spDamage, hitChance, move);
            }
            else
            {
                animHitCheck = false;
                //print("MISS!! " + hitChance + " vs " + rollForHit);
                atkStats.gameObject.GetComponent<Animator>().SetTrigger(move);
                damagePanel.GetComponent<DamagePanelManager>().ShowDamage(0, 0, hitChance, move);
            }
        }
        else
        {
            print("not enough SP");
        }
    }

    public void CalculateDistanceBetween()
    {
        distanceBetween = pstats.transform.position.x - estats.transform.position.x;
        battleRange = Mathf.RoundToInt(distanceBetween / 5f);
        battleRange = Mathf.Abs(battleRange);
    }

    public void FaceOpponent()
    {
        pArmature.transform.LookAt(eArmature.transform);
        eArmature.transform.LookAt(pArmature.transform);
    }
    public void DashForward(UnitBattleStats unit)
    {
        unit.gameObject.transform.Translate(Vector3.back * Time.deltaTime * 10);
        if (unit.gameObject.transform.position.x < minX)
        {
            unit.gameObject.transform.position = new Vector3(minX, 0, 0);
        }
        if (unit.gameObject.transform.position.x > maxX)
        {
            unit.gameObject.transform.position = new Vector3(maxX, 0, 0);
        }
    }

    public void DashBackward(UnitBattleStats unit)
    {
        unit.gameObject.transform.Translate(Vector3.back * Time.deltaTime * 10);
        if (unit.gameObject.transform.position.x < minX)
        {
            unit.gameObject.transform.position = new Vector3(minX, 0, 0);
        }
        if (unit.gameObject.transform.position.x > maxX)
        {
            unit.gameObject.transform.position = new Vector3(maxX, 0, 0);
        }
    }

    public void LoadStatsFromPlayerStats()
    {
        pstats.hpCurrent = PlayerStats.Instance.hpCurrent;
        pstats.hpMax = PlayerStats.Instance.hpMax;
        pstats.spCurrent = PlayerStats.Instance.spCurrent;
        pstats.spMax = PlayerStats.Instance.spMax;
        pstats.power = PlayerStats.Instance.power;
        pstats.intelligence = PlayerStats.Instance.intelligence;
        pstats.skill = PlayerStats.Instance.skill;
        pstats.speed = PlayerStats.Instance.speed;
        pstats.defense = PlayerStats.Instance.defense;
        pstats.resistance = PlayerStats.Instance.resistance;
        pstats.toughness = PlayerStats.Instance.toughness;
        pstats.stamina = PlayerStats.Instance.stamina;
    }


    public void SetEnemyStats()
    {
        int difMod = 1;
        if(PlayerStats.Instance.difficulty == "Easy")
        {
            difMod = 1;
        }
        else if (PlayerStats.Instance.difficulty == "Medium")
        {
            difMod = 2;
        }
        else if (PlayerStats.Instance.difficulty == "Hard")
        {
            difMod = 4;
        }

        estats.hpCurrent *= difMod;
        estats.hpMax *= difMod;
        estats.spCurrent *= difMod;
        estats.spMax *= difMod;
        estats.power *= difMod;
        estats.intelligence *= difMod;
        estats.skill *= difMod;
        estats.speed *= difMod;
        estats.defense *= difMod;
        estats.resistance *= difMod;
        estats.toughness *= difMod;
        estats.stamina *= difMod;

    }

    public void SpRegen()
    {
        pstats.spCurrent += pstats.spRegen * Time.deltaTime / 10;
        estats.spCurrent += estats.spRegen * Time.deltaTime / 10;

        if(pstats.spCurrent > pstats.spMax)
        {
            pstats.spCurrent = pstats.spMax;
        }

        if (estats.spCurrent > estats.spMax)
        {
            estats.spCurrent = estats.spMax;
        }
    }

}

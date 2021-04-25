using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class HomeManager : MonoBehaviour
{

    public enum HOMESTATE
    {
        WAIT,
        INITIALIZE,
        HOME,
        REPORT,
        EVENT,
        REST,
        ACTIVITYSELECT,
        TRAINSELECT,
        TRAINCONFIRM,
        MESSAGES,
        TRAINSEQUENCE,
        TRAINRESULTS,
        RESTCONFIRM,
        UPGRADESELECT,
        UPGRADECONFIRM,
        BATTLESELECT,
        RESULTS,
        END
    }
    public HOMESTATE homeState;

    [Header("Assign in Inspector")]
    public GameObject homePanel;
    public GameObject datePanel;
    public GameObject statusPanel;
    public GameObject trainPanel;
    public GameObject speechPanel;
    public GameObject confirmPanel;
    public GameObject reportPanel;
    public GameObject upgradePanel;
    public GameObject monsterPanel;
    public GameObject trainerPanel;
    public GameObject battlePanel;
    public GameObject monster;

    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip yesSound;
    public AudioClip noSound;
    public AudioClip selectSound;
    public AudioClip homeTheme;
    public float volume = 1f;

    public List<GameObject> allPanelsList = new List<GameObject>();

    public bool isActionOngoing = false;
    public string statToTrain;
    public string statToUpgrade;
    public int costToUpgrade;
    public string activitySelected;
    public string userAnswer;
    public string difficulty = "Easy";

    public SpeechContent speechToForm;
    public ReportContent reportToForm;

    // Start is called before the first frame update
    void Start()
    {
        //add all panels to list
        allPanelsList.Add(homePanel);
        allPanelsList.Add(datePanel);
        allPanelsList.Add(statusPanel);
        allPanelsList.Add(trainPanel);
        allPanelsList.Add(speechPanel);
        allPanelsList.Add(confirmPanel);
        allPanelsList.Add(reportPanel);
        allPanelsList.Add(upgradePanel);
        allPanelsList.Add(monsterPanel);
        allPanelsList.Add(trainerPanel);

        homeState = HOMESTATE.INITIALIZE;

        speechToForm = GameObject.Find("Speech").GetComponent<SpeechContent>();
        reportToForm = GameObject.Find("Report").GetComponent<ReportContent>();

        audioSource.clip = homeTheme;
        audioSource.loop = true;
        audioSource.Play();
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (homeState)
        {
            case HOMESTATE.INITIALIZE:

                if(isActionOngoing)
                {
                    return;
                }

                userAnswer = null;
                statToTrain = null;
                statToUpgrade = null;
                costToUpgrade = 0;

                DeactivateAllPanels();
                homeState = HOMESTATE.REPORT;

                break;

            case HOMESTATE.HOME:

                if (isActionOngoing)
                {
                    return;
                }

                userAnswer = null;
                statToTrain = null;
                statToUpgrade = null;
                costToUpgrade = 0;

                DeactivateAllPanels();
                homeState = HOMESTATE.ACTIVITYSELECT;

                break;

            case HOMESTATE.REPORT:
                if (!isActionOngoing)
                {
                    DeactivateAllPanels();
                    ClearReportList();
                    reportPanel.SetActive(true);
                    //tiredness
                    var sentence = "";
                    if(PlayerStats.Instance.tiredness <= 25)
                    {
                        sentence = PlayerStats.Instance.monsterName + " is feeling great!";
                    }
                    else if(PlayerStats.Instance.tiredness > 25 && PlayerStats.Instance.tiredness <= 50)
                    {
                        sentence = PlayerStats.Instance.monsterName + " is feeling well.";
                    }
                    else if (PlayerStats.Instance.tiredness > 50 && PlayerStats.Instance.tiredness <= 75)
                    {
                        sentence = PlayerStats.Instance.monsterName + " is tired.";
                    }
                    else if (PlayerStats.Instance.tiredness > 75)
                    {
                        sentence = PlayerStats.Instance.monsterName + " is extremely tired. Please give it some rest.";
                    }
                    ReportSomething("Danny", "Danny", sentence);

                    //stress
                    sentence = "";
                    if (PlayerStats.Instance.stress <= 25)
                    {
                        //sentence = PlayerStats.Instance.monsterName + " is feeling great!";
                    }
                    else if (PlayerStats.Instance.stress > 25 && PlayerStats.Instance.stress <= 50)
                    {
                        //sentence = PlayerStats.Instance.monsterName + " is feeling well.";
                    }
                    else if (PlayerStats.Instance.stress > 50 && PlayerStats.Instance.stress <= 75)
                    {
                        sentence = PlayerStats.Instance.monsterName + " is stressed out. We have to take care of it.";
                    }
                    else if (PlayerStats.Instance.stress > 75)
                    {
                        sentence = PlayerStats.Instance.monsterName + " is extremely stressed. Please give it some rest.";
                    }
                    if(sentence != "")
                    {
                        ReportSomething("Danny", "Danny", sentence);
                    }
                    PrintReports();
                    homeState = HOMESTATE.ACTIVITYSELECT;
                    isActionOngoing = true;
                }
                break;

            case HOMESTATE.EVENT:
                if (!isActionOngoing)
                {

                }
                break;

            case HOMESTATE.ACTIVITYSELECT:
                if (!isActionOngoing)
                {
                    DeactivateAllPanels();
                    homePanel.SetActive(true);
                    datePanel.SetActive(true);
                    statusPanel.SetActive(true);
                    datePanel.GetComponent<DatePanelManager>().PrintDate();
                    statusPanel.GetComponent<StatusPanelManager>().PrintStatus();
                    isActionOngoing = true;
                }
                break;

            case HOMESTATE.TRAINSELECT:
                isActionOngoing = false;
                if (userAnswer != null)
                {
                    homeState = HOMESTATE.TRAINCONFIRM;
                }
                break;

            case HOMESTATE.TRAINCONFIRM:
                switch (userAnswer)
                {
                    case "Yes":
                        DeactivateAllPanels();
                        ClearSpeechList();
                        var sentence = PlayerStats.Instance.monsterName + "! Let's train " + statToTrain + "!";
                        SaySomething("Danny", "Danny", sentence);
                        homeState = HOMESTATE.MESSAGES;
                        isActionOngoing = true;
                        break;

                    case "No":
                        homeState = HOMESTATE.HOME;
                        break;
                }
                break;

            case HOMESTATE.MESSAGES:
                if (!isActionOngoing)
                {
                    homeState = HOMESTATE.TRAINSEQUENCE;
                    //SceneLoader.Instance.ScreenTransition();
                }
                break;

            case HOMESTATE.TRAINSEQUENCE:
                if (!isActionOngoing)
                {
                    var increment = PlayerStats.Instance.TrainStat(statToTrain);
                    DeactivateAllPanels();
                    ClearSpeechList();
                    var sentence = statToTrain + " increased by " + increment +"!";
                    SaySomething("Danny", "Danny", sentence);
                    isActionOngoing = true;
                    homeState = HOMESTATE.TRAINRESULTS;
                }
                break;

            case HOMESTATE.TRAINRESULTS:
                if (!isActionOngoing)
                {
                    homeState = HOMESTATE.END;
                    //SceneLoader.Instance.ScreenTransition();
                }
                break;

            case HOMESTATE.RESTCONFIRM:
                isActionOngoing = false;
                switch (userAnswer)
                {
                    case "Yes":
                        PlayerStats.Instance.Rest();
                        DeactivateAllPanels();
                        ClearSpeechList();
                        var sentence = "Take it easy for now " + PlayerStats.Instance.monsterName + ". Rest well.";
                        SaySomething("Danny", "Danny", sentence);
                        
                        homeState = HOMESTATE.END;
                        isActionOngoing = true;
                        break;

                    case "No":
                        homeState = HOMESTATE.HOME;
                        break;
                }
                break;

            case HOMESTATE.UPGRADESELECT:
                isActionOngoing = false;
                if (userAnswer != null)
                {
                    //homeState = HOMESTATE.UPGRADECONFIRM;
                }
                break;

            case HOMESTATE.UPGRADECONFIRM:
                switch (userAnswer)
                {
                    case "Yes":
                        PlayerStats.Instance.UpgradeStat(statToUpgrade);
                        //subtract cost from funds
                        PlayerStats.Instance.funds -= costToUpgrade;
                        homeState = HOMESTATE.HOME;
                        break;

                    case "No":
                        homeState = HOMESTATE.HOME;
                        break;
                }
                break;

            case HOMESTATE.BATTLESELECT:
                isActionOngoing = false;
                if (userAnswer != null)
                {
                    homeState = HOMESTATE.UPGRADECONFIRM;
                }
                break;

            case HOMESTATE.RESULTS:
                if (!isActionOngoing)
                {
                    //speechPanel.GetComponent<SpeechPanelManager>().ExecuteSpeech(speechToForm);
                }
                break;
            case HOMESTATE.END:
                if (!isActionOngoing)
                {
                    //SceneLoader.Instance.ScreenToBlack();
                    PlayerStats.Instance.ageWeek++;
                    PlayerStats.Instance.week++;
                    StartCoroutine(LoadYourAsyncScene("Home"));
                    isActionOngoing = true;
                }
                break;
        }

    }

    IEnumerator LoadYourAsyncScene(string scene)
    {
        yield return new WaitForSeconds(3f); // wait 1s before loading, for sounds to finish playing
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void TriggerThisEvent(string eventToTrigger)
    {
        Invoke(eventToTrigger, 0f);
        print(eventToTrigger);
    }

    public void DeactivateAllPanels()
    {
        foreach (var item in allPanelsList)
        {
            item.SetActive(false);
            //print(item.name);
        }
    }
    public void ActivateAllPanels()
    {
        foreach (var item in allPanelsList)
        {
            item.SetActive(true);
        }
    }


    public void HomePanelButtonClicked(string buttonClicked)
    {
        audioSource.PlayOneShot(selectSound, volume);
        switch (buttonClicked)
        {
            case "Train":
                DeactivateAllPanels();
                trainPanel.SetActive(true);
                homeState = HOMESTATE.TRAINSELECT;
                monster.GetComponent<Animator>().SetTrigger("Backflip");
                break;

            case "Rest":
                DeactivateAllPanels();
                confirmPanel.SetActive(true);
                var sentence = PlayerStats.Instance.monsterName + " will rest for this week. Proceed?";
                AskSomething("Danny", "Danny", sentence);
                homeState = HOMESTATE.RESTCONFIRM;
                monster.GetComponent<Animator>().SetTrigger("Backflip");
                break;

            case "Upgrade":
                DeactivateAllPanels();
                upgradePanel.SetActive(true);
                homeState = HOMESTATE.UPGRADESELECT;
                monster.GetComponent<Animator>().SetTrigger("Backflip");
                break;

            case "Monster":
                DeactivateAllPanels();
                monsterPanel.SetActive(true);
                homeState = HOMESTATE.ACTIVITYSELECT;
                monster.GetComponent<Animator>().SetTrigger("Backflip");
                break;

            case "Trainer":
                DeactivateAllPanels();
                trainerPanel.SetActive(true);
                homeState = HOMESTATE.ACTIVITYSELECT;
                monster.GetComponent<Animator>().SetTrigger("Backflip");
                break;

            case "Battle":
                DeactivateAllPanels();
                battlePanel.SetActive(true);
                homeState = HOMESTATE.BATTLESELECT;
                monster.GetComponent<Animator>().SetTrigger("Backflip");
                break;
        }

    }

    public void TrainPanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        statToTrain = userAnswer;
        DeactivateAllPanels();
        confirmPanel.SetActive(true);
        var sentence = "This training raises " + buttonClicked + ". Do you want to proceed?";
        AskSomething("Danny", "Danny", sentence);
        homeState = HOMESTATE.TRAINCONFIRM;
        audioSource.PlayOneShot(selectSound, volume);

        if (userAnswer == "Cancel")
        {
            homeState = HOMESTATE.HOME;
            isActionOngoing = false;

        }
    }

    public void MonsterPanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        audioSource.PlayOneShot(selectSound, volume);

        if (userAnswer == "Cancel")
        {
            homeState = HOMESTATE.HOME;
            isActionOngoing = false;
        }
    }

    public void TrainerPanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        audioSource.PlayOneShot(selectSound, volume);

        if (userAnswer == "Cancel")
        {
            homeState = HOMESTATE.HOME;
            isActionOngoing = false;
        }
    }

    public void BattlePanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        audioSource.PlayOneShot(selectSound, volume);

        if (userAnswer == "Cancel")
        {
            homeState = HOMESTATE.HOME;
            battlePanel.SetActive(false);
            isActionOngoing = false;
        }
        else if (userAnswer == "Easy")
        {
            SceneLoader.Instance.LoadScene("Battle");
            isActionOngoing = false;
            PlayerStats.Instance.difficulty = "Easy";
        }
        else if (userAnswer == "Medium")
        {
            SceneLoader.Instance.LoadScene("Battle");
            isActionOngoing = false;
            PlayerStats.Instance.difficulty = "Medium";
        }
        else if (userAnswer == "Hard")
        {
            SceneLoader.Instance.LoadScene("Battle");
            isActionOngoing = false;
            PlayerStats.Instance.difficulty = "Hard";
        }
    }

    


    public void UpgradePanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        statToUpgrade = userAnswer;
        DeactivateAllPanels();
        confirmPanel.SetActive(true);
        //check cost
        costToUpgrade = PlayerStats.Instance.CostToUpgrade(statToUpgrade);
        if(PlayerStats.Instance.funds > costToUpgrade)
        {
            var sentence = "Upgrading " + statToUpgrade + " costs " + costToUpgrade + " funds. Proceed?";
            AskSomething("Danny", "Danny", sentence);
            //confirmPanel.GetComponent<ConfirmPanelManager>().
            //    ExecuteQuestion("Upgrading " + statToUpgrade + " costs " + costToUpgrade + " funds. Proceed?");

            homeState = HOMESTATE.UPGRADECONFIRM;
        }
        else
        {
            var sentence = "Upgrading " + statToUpgrade + " costs " + costToUpgrade + " funds. We have insufficient funds.";
            AskSomething("Danny", "Danny", sentence);
            //confirmPanel.GetComponent<ConfirmPanelManager>().
            //    ExecuteQuestion("Upgrading " + statToUpgrade + " costs " + costToUpgrade + " funds. We have insufficient funds.");
            homeState = HOMESTATE.HOME;
        }

        if (userAnswer == "Cancel")
        {
            homeState = HOMESTATE.HOME;
            isActionOngoing = false;
        }
        audioSource.PlayOneShot(selectSound, volume);
    }

    public void ConfirmPanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        if(userAnswer == "Yes")
        {
            audioSource.PlayOneShot(yesSound, volume);
        }
        else if (userAnswer == "No")
        {
            audioSource.PlayOneShot(noSound, volume);
        }
    }

    public void ReportPanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        if (userAnswer == "ReportPanelNextButton")
        {
            audioSource.PlayOneShot(yesSound, volume);
        }
    }

    public void SpeechPanelButtonClicked(string buttonClicked)
    {
        userAnswer = buttonClicked;
        if (userAnswer == "SpeechPanelNextButton")
        {
            audioSource.PlayOneShot(yesSound, volume);
        }
    }

    public void ClearSpeechList()
    {
        if (speechToForm != null)
        {
            speechToForm.speech.Clear();
        }
    }
    public void ClearReportList()
    {
        if(reportToForm != null)
        {
            reportToForm.report.Clear();
        }
    }

    public void SaySomething(string speaker, string image, string sentence)
    {
        speechToForm.speakerName = speaker;
        speechToForm.speakerImage = Resources.Load<Sprite>(image);
        //speechToForm.speech.Clear();
        speechToForm.speech.Add(sentence);
        speechPanel.GetComponent<SpeechPanelManager>().ExecuteSpeech(speechToForm);
    }

    public void ReportSomething(string title, string image, string sentence)
    {
        reportToForm.reportName = title;
        reportToForm.reportImage = Resources.Load<Sprite>(image);
        //reportToForm.report.Clear();
        reportToForm.report.Add(sentence);
        
    }

    public void PrintReports()
    {
        reportPanel.GetComponent<ReportPanelManager>().ExecuteReport(reportToForm);
    }

    public void AskSomething(string speaker, string image, string sentence)
    {
        var confirmSpeaker = speaker;
        var confirmImage = Resources.Load<Sprite>(image);
        var confirmQuestion = sentence;
        confirmPanel.GetComponent<ConfirmPanelManager>().ExecuteQuestion(confirmSpeaker, confirmImage, confirmQuestion);
    }


}

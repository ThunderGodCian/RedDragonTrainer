using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechPanelManager : MonoBehaviour
{

//# region singleton
//    private static SpeechPanelManager _instance;
//    public static SpeechPanelManager Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = GameObject.FindObjectOfType<SpeechPanelManager>();
//            }

//            return _instance;
//        }
//    }

//    void Awake()
//    {
//        if (_instance != null && _instance != this)
//            Destroy(this.gameObject);
//        else
//        {
//            _instance = this;
//            DontDestroyOnLoad(this.gameObject);
//        }

//        DontDestroyOnLoad(gameObject);
//    }
//    #endregion singleton


    public GameObject speechPanel;
    public Text speakerNameText;
    public Image speakerImage;
    public Text speechText;
    public Button speechNextButton;

    public int speechIndex = 0;
    public SpeechContent speechContent;

    public HomeManager homeManager;

    // Start is called before the first frame update
    void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        //initialize, map objects to fields
        speechPanel = GameObject.Find("SpeechPanel");
        speechText = GameObject.Find("SpeechBoxText").GetComponent<Text>();
        speakerNameText = GameObject.Find("SpeakerNameText").GetComponent<Text>();
        speakerImage = GameObject.Find("SpeakerImage").GetComponent<Image>();
        speechNextButton = GameObject.Find("SpeechPanelNextButton").GetComponent<Button>();
        speechNextButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked(speechNextButton.transform.name));

    }

    private void Update()
    {
        
        if(speechContent == null)
        {
            return;
        }

        if (speechIndex >= speechContent.speech.Count)
        {
            homeManager.isActionOngoing = false;
            speechPanel.SetActive(false);
        }
    }

    public void ExecuteSpeech(SpeechContent item)
    {
        speechPanel = this.gameObject;
        speechPanel.SetActive(true);
        speechIndex = 0; //reset index
        speechContent = item;
        StartCoroutine(DisplaySpeech());
    }

    public IEnumerator DisplaySpeech()
    {
        speechText = GameObject.Find("SpeechBoxText").GetComponent<Text>();
        speakerNameText = GameObject.Find("SpeakerNameText").GetComponent<Text>();
        speakerImage = GameObject.Find("SpeakerImage").GetComponent<Image>();

        while (speechIndex < speechContent.speech.Count)
        {
            speakerImage.sprite = speechContent.speakerImage;
            speakerNameText.text = speechContent.speakerName.ToString();
            speechText.text = speechContent.speech[speechIndex];
            yield return new WaitForEndOfFrame();
        }

    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.SpeechPanelButtonClicked(itemName);
        IncrementSpeechIndex();
    }

    public void IncrementSpeechIndex()
    {
        speechIndex++;
    }
}

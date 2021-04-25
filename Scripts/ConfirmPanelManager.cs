using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanelManager : MonoBehaviour
{
    public GameObject confirmPanel;
    public Text confirmNameText;
    public Image confirmImage;
    public Text confirmText;
    public Button confirmYesButton;
    public Button confirmNoButton;

    public HomeManager homeManager;

    // Start is called before the first frame update
    void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        //initialize, map objects to fields
        confirmPanel = GameObject.Find("ConfirmPanel");
        confirmText = GameObject.Find("ConfirmBoxText").GetComponent<Text>();
        confirmYesButton = GameObject.Find("ConfirmYesButton").GetComponent<Button>();
        confirmYesButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("Yes"));
        confirmNoButton = GameObject.Find("ConfirmNoButton").GetComponent<Button>();
        confirmNoButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("No"));

    }

    public void ExecuteQuestion(string name, Sprite image, string question)
    {
        confirmText = GameObject.Find("ConfirmBoxText").GetComponent<Text>();
        confirmNameText = GameObject.Find("ConfirmNameText").GetComponent<Text>();
        confirmImage = GameObject.Find("ConfirmImage").GetComponent<Image>();

        confirmImage.sprite = image;
        confirmNameText.text = name;        
        confirmText.text = question;
    }

    public void MenuItemClicked(string item)
    {
        homeManager.ConfirmPanelButtonClicked(item);
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);

    }
    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

}

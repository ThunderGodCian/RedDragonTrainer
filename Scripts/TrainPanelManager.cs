using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainPanelManager : MonoBehaviour
{
    public List<string> TrainPanelContent = new List<string>();

    public GameObject TrainPanelButtonPrefab;
    public GameObject TrainPanelOrganizer;
    public GameObject TrainPanelCancelButton;

    public HomeManager homeManager;


    private void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        TrainPanelButtonPrefab = Resources.Load("Prefabs/TrainPanelButtonTemplate") as GameObject;
        TrainPanelOrganizer = GameObject.Find("TrainPanelOrganizer");
        TrainPanelCancelButton = GameObject.Find("TrainPanelCancelButton");
        TrainPanelCancelButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("Cancel"));

        TrainPanelContent.Add("Power");
        TrainPanelContent.Add("Intelligence");
        TrainPanelContent.Add("Skill");
        TrainPanelContent.Add("Speed");
        TrainPanelContent.Add("Defense");
        TrainPanelContent.Add("Resistance");
        TrainPanelContent.Add("Toughness");
        TrainPanelContent.Add("Stamina");
        TrainPanelContent.Add("Technique");

        //clear menu before populating
        foreach (Transform child in TrainPanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in TrainPanelContent)
        {
            //instantiate button template in organizer
            GameObject trainPanelItem = Instantiate(TrainPanelButtonPrefab, TrainPanelOrganizer.transform);
            trainPanelItem.transform.name = menuItem;
            trainPanelItem.transform.Find("TrainPanelButtonText").GetComponent<Text>().text = menuItem + "";
            trainPanelItem.transform.Find("TrainPanelButtonText").name = menuItem + "ButtonText";
            trainPanelItem.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked(trainPanelItem.transform.name));

        }

    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.TrainPanelButtonClicked(itemName);
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

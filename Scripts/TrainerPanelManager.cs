using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerPanelManager : MonoBehaviour
{
    public List<string> TrainerPanelContent = new List<string>();

    public GameObject TrainerPanelButtonPrefab;
    public GameObject TrainerPanelOrganizer;
    public GameObject TrainerPanelCancelButton;

    public HomeManager homeManager;


    private void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        TrainerPanelButtonPrefab = Resources.Load("Prefabs/TrainerPanelButtonTemplate") as GameObject;
        TrainerPanelOrganizer = GameObject.Find("TrainerPanelOrganizer");
        TrainerPanelCancelButton = GameObject.Find("TrainerPanelCancelButton");
        TrainerPanelCancelButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("Cancel"));

        TrainerPanelContent.Add("PlayerName");
        TrainerPanelContent.Add("TrainingLevel");
        TrainerPanelContent.Add("RestLevel");
        TrainerPanelContent.Add("EarthLevel");
        TrainerPanelContent.Add("FundsLevel");

        //clear menu before populating
        foreach (Transform child in TrainerPanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in TrainerPanelContent)
        {
            //instantiate button template in organizer
            GameObject TrainerPanelItem = Instantiate(TrainerPanelButtonPrefab, TrainerPanelOrganizer.transform);
            TrainerPanelItem.transform.name = menuItem;
            TrainerPanelItem.transform.Find("TrainerPanelButtonText").GetComponent<Text>().text =
                menuItem + ": " +
                PlayerStats.Instance.ReturnStatValue(menuItem).ToString();
            TrainerPanelItem.transform.Find("TrainerPanelButtonText").name = menuItem;
        }
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.TrainerPanelButtonClicked(itemName);
    }
}

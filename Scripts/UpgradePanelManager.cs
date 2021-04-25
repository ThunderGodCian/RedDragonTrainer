using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelManager : MonoBehaviour
{
    public List<string> UpgradePanelContent = new List<string>();

    public GameObject UpgradePanelButtonPrefab;
    public GameObject UpgradePanelOrganizer;
    public GameObject UpgradePanelCancelButton;

    public HomeManager homeManager;


    private void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        UpgradePanelButtonPrefab = Resources.Load("Prefabs/UpgradePanelButtonTemplate") as GameObject;
        UpgradePanelOrganizer = GameObject.Find("UpgradePanelOrganizer");
        UpgradePanelCancelButton = GameObject.Find("UpgradePanelCancelButton");
        UpgradePanelCancelButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("Cancel"));

        UpgradePanelContent.Add("TrainingLevel");
        UpgradePanelContent.Add("RestLevel");
        UpgradePanelContent.Add("EarthLevel");
        UpgradePanelContent.Add("FundsLevel");

        //clear menu before populating
        foreach (Transform child in UpgradePanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in UpgradePanelContent)
        {
            //instantiate button template in organizer
            GameObject trainPanelItem = Instantiate(UpgradePanelButtonPrefab, UpgradePanelOrganizer.transform);
            trainPanelItem.transform.name = menuItem;
            trainPanelItem.transform.Find("UpgradePanelButtonText").GetComponent<Text>().text = menuItem + "";
            trainPanelItem.transform.Find("UpgradePanelButtonText").name = menuItem + "ButtonText";
            trainPanelItem.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked(trainPanelItem.transform.name));

        }

    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.UpgradePanelButtonClicked(itemName);
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
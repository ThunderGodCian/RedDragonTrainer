using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanelManager : MonoBehaviour
{
    public List<string> BattlePanelContent = new List<string>();

    public GameObject BattlePanelButtonPrefab;
    public GameObject BattlePanelOrganizer;
    public GameObject BattlePanelCancelButton;

    public HomeManager homeManager;


    private void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        BattlePanelButtonPrefab = Resources.Load("Prefabs/BattlePanelButtonTemplate") as GameObject;
        BattlePanelOrganizer = GameObject.Find("BattlePanelOrganizer");
        BattlePanelCancelButton = GameObject.Find("BattlePanelCancelButton");
        BattlePanelCancelButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("Cancel"));

        BattlePanelContent.Add("Easy");
        BattlePanelContent.Add("Medium");
        BattlePanelContent.Add("Hard");

        //clear menu before populating
        foreach (Transform child in BattlePanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in BattlePanelContent)
        {
            //instantiate button template in organizer
            GameObject trainPanelItem = Instantiate(BattlePanelButtonPrefab, BattlePanelOrganizer.transform);
            trainPanelItem.transform.name = menuItem;
            trainPanelItem.transform.Find("BattlePanelButtonText").GetComponent<Text>().text = menuItem + "";
            trainPanelItem.transform.Find("BattlePanelButtonText").name = menuItem + "ButtonText";
            trainPanelItem.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked(trainPanelItem.transform.name));

        }

    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.BattlePanelButtonClicked(itemName);
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

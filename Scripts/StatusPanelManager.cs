using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanelManager : MonoBehaviour
{
    public List<string> StatusPanelContent = new List<string>();

    public GameObject StatusPanelButtonPrefab;
    public GameObject StatusPanelOrganizer;

    public HomeManager homeManager;

    public Text worldHp;
    public Text funds;

    private void Awake()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();
        //add to panel list
        //homeManager.allPanelsList.Add(gameObject);

        StatusPanelButtonPrefab = Resources.Load("Prefabs/StatusPanelButtonTemplate") as GameObject;
        StatusPanelOrganizer = GameObject.Find("StatusPanelOrganizer");

        StatusPanelContent.Add("World Integrity");
        StatusPanelContent.Add("Funds");

        //clear menu before populating
        foreach (Transform child in StatusPanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in StatusPanelContent)
        {
            //instantiate button template in organizer
            GameObject StatusPanelItem = Instantiate(StatusPanelButtonPrefab, StatusPanelOrganizer.transform);
            StatusPanelItem.transform.name = menuItem;
            StatusPanelItem.transform.Find("StatusPanelButtonText").GetComponent<Text>().text = menuItem + ": XX";
            StatusPanelItem.transform.Find("StatusPanelButtonText").name = menuItem + "ButtonText";
        }

        worldHp = GameObject.Find("World IntegrityButtonText").GetComponent<Text>();
        worldHp.text = "World Integrity: " + PlayerStats.Instance.worldHp + "%";
        funds = GameObject.Find("FundsButtonText").GetComponent<Text>();
        funds.text = "Funds: " + PlayerStats.Instance.funds;
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void PrintStatus()
    {
        if (worldHp != null)
        {
            worldHp = GameObject.Find("World IntegrityButtonText").GetComponent<Text>();
            worldHp.text = "World Integrity: " + PlayerStats.Instance.worldHp + "%";
            funds = GameObject.Find("FundsButtonText").GetComponent<Text>();
            funds.text = "Funds: " + PlayerStats.Instance.funds;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPanelManager : MonoBehaviour
{
    public List<string> MonsterPanelContent = new List<string>();

    public GameObject MonsterPanelButtonPrefab;
    public GameObject MonsterPanelOrganizer;
    public GameObject MonsterPanelCancelButton;

    public HomeManager homeManager;


    private void Awake()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        MonsterPanelButtonPrefab = Resources.Load("Prefabs/MonsterPanelButtonTemplate") as GameObject;
        MonsterPanelOrganizer = GameObject.Find("MonsterPanelOrganizer");
        MonsterPanelCancelButton = GameObject.Find("MonsterPanelCancelButton");
        MonsterPanelCancelButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked("Cancel"));

        MonsterPanelContent.Add("MonsterName");
        MonsterPanelContent.Add("Power");
        MonsterPanelContent.Add("Intelligence");
        MonsterPanelContent.Add("Skill");
        MonsterPanelContent.Add("Speed");
        MonsterPanelContent.Add("Defense");
        MonsterPanelContent.Add("Resistance");
        MonsterPanelContent.Add("Toughness");
        MonsterPanelContent.Add("Stamina");
        MonsterPanelContent.Add("AgeYear");
        MonsterPanelContent.Add("AgeMonth");


        //clear menu before populating
        foreach (Transform child in MonsterPanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in MonsterPanelContent)
        {
            //instantiate button template in organizer
            GameObject MonsterPanelItem = Instantiate(MonsterPanelButtonPrefab, MonsterPanelOrganizer.transform);
            MonsterPanelItem.transform.name = menuItem;
            MonsterPanelItem.transform.Find("MonsterPanelButtonText").GetComponent<Text>().text =
                menuItem + ": " +
                PlayerStats.Instance.ReturnStatValue(menuItem).ToString();
            MonsterPanelItem.transform.Find("MonsterPanelButtonText").name = menuItem;
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
        homeManager.MonsterPanelButtonClicked(itemName);
    }
}
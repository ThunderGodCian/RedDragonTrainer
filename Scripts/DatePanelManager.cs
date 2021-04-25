using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatePanelManager : MonoBehaviour
{
    public List<string> DatePanelContent = new List<string>();

    public GameObject DatePanelButtonPrefab;
    public GameObject DatePanelOrganizer;

    public HomeManager homeManager;

    public Text year;
    public Text month;
    public Text week;

    private void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        DatePanelButtonPrefab = Resources.Load("Prefabs/DatePanelButtonTemplate") as GameObject;
        DatePanelOrganizer = GameObject.Find("DatePanelOrganizer");

        DatePanelContent.Add("Year");
        DatePanelContent.Add("Month");
        DatePanelContent.Add("Week");

        //clear menu before populating
        foreach (Transform child in DatePanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in DatePanelContent)
        {
            //instantiate button template in organizer
            GameObject DatePanelItem = Instantiate(DatePanelButtonPrefab, DatePanelOrganizer.transform);
            DatePanelItem.transform.name = menuItem;
            DatePanelItem.transform.Find("DatePanelButtonText").GetComponent<Text>().text = menuItem + " XX";
            DatePanelItem.transform.Find("DatePanelButtonText").name = menuItem + "ButtonText";
        }
        year = GameObject.Find("YearButtonText").GetComponent<Text>();
        year.text = "Year " + PlayerStats.Instance.year;
        month = GameObject.Find("MonthButtonText").GetComponent<Text>();
        month.text = "Month " + PlayerStats.Instance.month;
        week = GameObject.Find("WeekButtonText").GetComponent<Text>();
        week.text = "Week " + PlayerStats.Instance.week;
    }

    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
    public void PrintDate()
    {
        if(year != null)
        {
            year = GameObject.Find("YearButtonText").GetComponent<Text>();
            year.text = "Year" + PlayerStats.Instance.year;
            month = GameObject.Find("MonthButtonText").GetComponent<Text>();
            month.text = "Month" + PlayerStats.Instance.month;
            week = GameObject.Find("WeekButtonText").GetComponent<Text>();
            week.text = "Week" + PlayerStats.Instance.week;
        }

    }
}

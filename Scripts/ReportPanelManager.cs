using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportPanelManager : MonoBehaviour
{

    public GameObject reportPanel;
    public Text reportNameText;
    public Image reportImage;
    public Text reportText;
    public Button reportNextButton;

    public int reportIndex = 0;
    public ReportContent reportContent;

    public HomeManager homeManager;

    // Start is called before the first frame update
    void Awake()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();

        //initialize, map objects to fields
        reportPanel = GameObject.Find("ReportPanel");
        reportText = GameObject.Find("ReportPanelBoxText").GetComponent<Text>();
        reportNameText = GameObject.Find("ReportPanelNameText").GetComponent<Text>();
        reportImage = GameObject.Find("ReportImage").GetComponent<Image>();
        reportNextButton = GameObject.Find("ReportPanelNextButton").GetComponent<Button>();
        reportNextButton.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked(reportNextButton.transform.name));

    }

    private void Update()
    {
        if (reportContent == null)
        {
            return;
        }

        if (reportIndex >= reportContent.report.Count)
        {
            homeManager.isActionOngoing = false;
            //reportPanel.SetActive(false);
        }
    }


    public void ExecuteReport(ReportContent item)
    {
        reportIndex = 0; //reset index
        reportContent = item;
        reportPanel.SetActive(true);
        StartCoroutine(DisplayReport());
    }

    public IEnumerator DisplayReport()
    {

        reportText = GameObject.Find("ReportPanelBoxText").GetComponent<Text>();
        reportNameText = GameObject.Find("ReportPanelNameText").GetComponent<Text>();
        reportImage = GameObject.Find("ReportImage").GetComponent<Image>();

        while (reportIndex < reportContent.report.Count)
        {
            reportImage.sprite = reportContent.reportImage;
            reportNameText.text = reportContent.reportName.ToString();
            reportText.text = reportContent.report[reportIndex];
            yield return new WaitForEndOfFrame();
        }

    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.ReportPanelButtonClicked(itemName);
        IncrementReportIndex();
    }

    public void IncrementReportIndex()
    {
        reportIndex++;
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

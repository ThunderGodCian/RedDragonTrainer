using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanelManager : MonoBehaviour
{

    public List<string> homePanelContent = new List<string>();
    public List<GameObject> panelLinksList = new List<GameObject>();

    public GameObject homePanelButtonPrefab;
    public GameObject homePanelOrganizer;

    public HomeManager homeManager;

    private void Start()
    {
        homeManager = GameObject.Find("HomeManager").GetComponent<HomeManager>();
        //add to panel list
        //homeManager.allPanelsList.Add(gameObject);

        //initialize
        homePanelButtonPrefab = Resources.Load("Prefabs/HomePanelButtonTemplate") as GameObject;
        homePanelOrganizer = GameObject.Find("HomePanelOrganizer");

        homePanelContent.Add("Train");
        homePanelContent.Add("Rest");
        homePanelContent.Add("Battle");
        homePanelContent.Add("Upgrade");
        homePanelContent.Add("Monster");
        homePanelContent.Add("Trainer");

        //clear menu before populating
        foreach (Transform child in homePanelOrganizer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string menuItem in homePanelContent)
        {
            //instantiate button template in organizer
            GameObject homePanelItem = Instantiate(homePanelButtonPrefab, homePanelOrganizer.transform);
            homePanelItem.transform.name = menuItem;
            homePanelItem.transform.Find("HomePanelButtonText").GetComponent<Text>().text = menuItem;
            homePanelItem.transform.Find("HomePanelButtonText").name = menuItem + "ButtonText";
            homePanelItem.GetComponent<Button>().onClick.AddListener(() => MenuItemClicked(homePanelItem.transform.name));

            //form links to panels
            panelLinksList.Add(GameObject.Find(homePanelItem.transform.name + "Panel"));
            //close panels first
            foreach (GameObject panel in panelLinksList)
            {
                if(panel != null)
                {
                    panel.SetActive(false);
                }
            }
        }
        
    }

    public void MenuItemClicked(string itemName)
    {
        homeManager.HomePanelButtonClicked(itemName);
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

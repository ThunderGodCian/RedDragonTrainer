using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementPanelManager : MonoBehaviour
{
    public Button moveForward;
    public Button moveBackward;
    public Button dashForward;
    public Button dashBackward;

    public UnitController pController;

    public bool isForward = false;
    public bool isBackward = false;

    private void Start()
    {
        moveForward.onClick.AddListener(MoveForward);
        moveBackward.onClick.AddListener(MoveBackward);
        dashForward.onClick.AddListener(DashForward);
        dashBackward.onClick.AddListener(DashBackward);
    }

    private void Update()
    {
        if(isForward)
        {
            pController.MoveForward();
        }
        if (isBackward)
        {
            pController.MoveBackward();
        }
    }

    public void  MoveForward()
    {
        isForward = !isForward;
        isBackward = false;
    }
    public void MoveBackward()
    {
        isBackward = !isBackward;
        isForward = false;
    }
    public void DashForward()
    {

    }
    public void DashBackward()
    {

    }


}

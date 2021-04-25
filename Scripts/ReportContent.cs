using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportContent : MonoBehaviour
{
    public string reportName;

    public Sprite reportImage;

    [TextArea(3, 10)]
    public List<string> report = new List<string>();
    //public string report;
}

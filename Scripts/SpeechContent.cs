using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechContent : MonoBehaviour
{
    public string speakerName;

    public Sprite speakerImage;

    [TextArea(3, 10)]
    public List<string> speech = new List<string>();
}

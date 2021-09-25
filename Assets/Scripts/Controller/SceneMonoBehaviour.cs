using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneMonoBehaviour : MonoBehaviour
{
    public TMP_Text tourText;
    public TMP_Text locationText;
    public TMP_Text sceneText;

    // Start is called before the first frame update
    protected void Start()
    {
        SetSceneInfo();
    }

    protected void SetSceneInfo()
    {
        locationText.text = (string)ToursInfo.CurrentLocation["Name"];
        sceneText.text = (string)ToursInfo.CurrentSceneData["name"];
        tourText.text = (string)ToursInfo.CurrentTour["Name"];
    }
}

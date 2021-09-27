using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SceneMonoBehaviour : MonoBehaviour
{
    public TMP_Text tourText;
    public TMP_Text locationText;
    public TMP_Text sceneText;
    public float timeBetween = 6.0f;
    public GameObject SceneControllerContainer;
    protected SceneController sceneController;

    // Start is called before the first frame update
    protected void Start()
    {
        sceneController = SceneControllerContainer.GetComponent<SceneController>();
        SetSceneInfo();
    }

    protected void SetSceneInfo()
    {
        locationText.text = (string)ToursInfo.CurrentLocation["Name"];
        sceneText.text = (string)ToursInfo.CurrentSceneData["name"];
        tourText.text = (string)ToursInfo.CurrentTour["Name"];
    }

    public IEnumerator Timer(UnityAction<float> onTimer)
    {
        yield return new WaitForSeconds(timeBetween);
        if (onTimer != null)
        {
            onTimer.Invoke(timeBetween);
        }
        yield return null;
    }
}

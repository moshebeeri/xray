using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Augmented360Video : MonoBehaviour
{
    [Header("Scripts")]
    public GameObject stateManagerContainer;
    public GameObject videoManagerContainer;
    StateManager stateManager;
    VideoManager videoManager;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();
        videoManager = videoManagerContainer.GetComponent<VideoManager>();
        DownloadAndPlayVideo();
    }

    void DownloadAndPlayVideo()
    {
        // Dictionary<string, object> location = ToursInfo.CurrentTourLocations[ToursInfo.CurrentLocationIndex];
        // Dictionary<string, object> locationData = await stateManager.GetLocationById((string)location["Id"]);
        Dictionary<string, object> scene = ToursInfo.CurrentScene;
        if(!scene.ContainsKey("url"))
            // videoManager.url((string)locationData["Video360"]);
            return;
        string url = (string)scene["url"];
        string name = String.Format("{0}.mp4", url.GetHashCode());
        StartCoroutine(videoManager.DownloadAndPlayVideo((string)scene["url"], name));
    }
}

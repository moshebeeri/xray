using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video360Controller : MonoBehaviour
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
        Dictionary<string, object> scene = ToursInfo.CurrentSceneData;
        if(!scene.ContainsKey("url"))
            return;
        StartCoroutine(videoManager.DownloadAndPlayVideo((string)scene["url"]));
    }
}

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

    void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();
        videoManager = videoManagerContainer.GetComponent<VideoManager>();
        DownloadAndPlayVideo();
    }

    void DownloadAndPlayVideo()
    {
        Dictionary<string, object> scene = ToursInfo.CurrentScene;
        if(!scene.ContainsKey("url"))
            return;
        string url = (string)scene["url"];
        string name = String.Format("{0}.mp4", url.GetHashCode().ToString("X"));
        StartCoroutine(videoManager.DownloadAndPlayVideo(url, name));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video2DController : MonoBehaviour
{
    [Header("Scripts")]
    public GameObject stateManagerContainer;
    public GameObject videoManagerContainer;
    public GameObject SceneControllerContainer;
    StateManager stateManager;
    VideoManager videoManager;
    SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();
        videoManager = videoManagerContainer.GetComponent<VideoManager>();
        sceneController = SceneControllerContainer.GetComponent<SceneController>();
        DownloadAndPlayVideo();
    }

    void OnSceneEnded(string name)
    {
        Debug.Log(String.Format("On '${0}' Scene Ended", name));
        sceneController.NextScene();
    }

    void DownloadAndPlayVideo()
    {
        Dictionary<string, object> scene = ToursInfo.CurrentSceneData;
        if(!scene.ContainsKey("url"))
            return;
        StartCoroutine(videoManager.DownloadAndPlayVideo((string)scene["url"], OnSceneEnded));
    }
}

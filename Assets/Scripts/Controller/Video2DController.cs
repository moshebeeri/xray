using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video2DController : SceneMonoBehaviour
{
    [Header("Scripts")]
    public GameObject stateManagerContainer;
    public GameObject videoManagerContainer;
    StateManager stateManager;
    VideoManager videoManager;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        stateManager = stateManagerContainer.GetComponent<StateManager>();
        videoManager = videoManagerContainer.GetComponent<VideoManager>();
        sceneController = SceneControllerContainer.GetComponent<SceneController>();
        DownloadAndPlayVideo();
    }

    void OnSceneEnded(string name)
    {
        Debug.Log(String.Format("On '${0}' Scene Ended", name));
        if(sceneController)
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

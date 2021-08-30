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

    // Update is called once per frame
    void Update()
    {

    }

    async void DownloadAndPlayVideo()
    {
        Dictionary<string, object> location = ToursInfo.CurrentTourLocations[ToursInfo.CurrentLocationIndex];
        Dictionary<string, object> locationData = await stateManager.GetLocationById((string)location["Id"]);
        if(location.ContainsKey("Video360"))
            videoManager.url((string)locationData["Video360"]);
    }
}

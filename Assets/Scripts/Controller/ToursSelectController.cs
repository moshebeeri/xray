using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ToursSelectController : MonoBehaviour
{
    [Header("Preview")]
    public GameObject tourPreviewPrefab;
    public GameObject scrollView;
    [Header("StateManager")]
    public GameObject stateManagerContainer;
    protected StateManager stateManager;

    [Header("Debug")]
    public TMP_Text debugText;

    // Start is called before the first frame update
    async void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();
        List<Dictionary<string, object>> tours = await stateManager.FetchTours();


        if(tourPreviewPrefab && scrollView)
        {
			foreach (Dictionary<string, object> tour in tours)
            {
                if(!tour.ContainsKey("Name"))
                {
                    Debug.Log(String.Format("XRAY: Name not found in dictionary"));
                    continue;
                }
                Debug.Log(String.Format("XRAY: Name: {0}", tour["Name"]));
                GameObject preview = Instantiate(tourPreviewPrefab) as GameObject;
                preview.SetActive(true);
                preview.GetComponent<TourPreview>().FromDictionary(tour);
                string tourId = (string)tour["Id"];
				ToursInfo.Tours[tourId] = tour;
				preview.GetComponent<Button>().onClick.AddListener(() => OnPreviewClicked(tourId));
                TMP_Text title = preview.transform.Find("Title").GetComponent<TMP_Text>();
                if(title != null)
                {
                    title.text = (string)tour["Name"];
                }
                else
                {
                    title.text = "No Title";
                }
                preview.transform.SetParent(scrollView.transform, false);
            }
        }
        else
        {
            Debug.Log("XRAY: tourPreviewPrefab and scrollView should be populated");
        }
    }

    public void OnPreviewClicked(string tourId)
    {
        ToursInfo.CurrentTourId = tourId;
        ToursInfo.CurrentTour = ToursInfo.Tours[tourId];
        Task task = PrepareAndLaunchTourAsync(tourId);
    }
    async Task PrepareAndLaunchTourAsync(string tourId)
    {
        Debug.Log(String.Format("XRAY: PrepareAndLaunchTourAsync: tourId {0}", tourId));
        List<Dictionary<string, object>> locations = await stateManager.FetchTourLocations(tourId);
        ToursInfo.TourLocations = locations;
        ToursInfo.CurrentLocationIndex = 0;
        Dictionary<string, object> location = ToursInfo.Location();
        Dictionary<string, object> locationData = await stateManager.FetchLocation(location["Location"]);
        ToursInfo.CurrentLocation = locationData;
        List<object> scenes = (List<object>)locationData["scenes"];
        Debug.Log(String.Format("XRAY: PrepareAndLaunchTourAsync {0} scenes in location {1}", scenes.Count, (string)location["Id"]));
        if(scenes.Count > 0)
        {
            Dictionary<string, object> sceneData = await stateManager.FetchScene(scenes[0]);
            ToursInfo.CurrentSceneData = sceneData;
            Loader.LoadScene(sceneData);
        }
    }

    // public async void PopulateTours()
    // {
    //     List<Dictionary<string, object>> tours = await stateManager.FetchTours();
    //     Debug.Log(String.Format("XRAY: Number of tours {0}", tours.Count));
    //     foreach (Dictionary<string, object> tour in tours)
    //     {
    //         Debug.Log(String.Format("XRAY: Name: {0}", tour["Name"]));
    //     }
    // }

    // void FixedUpdate() {
    //     GameObject preview = Instantiate(tourPreviewPrefab, new Vector3(0,0,0), Quaternion.identity);
    //     preview.GetComponent<Rigidbody>().velocity = getRandomForce();
    // }

    // private Vector3 getRandomForce()
    // {
    //     float x =  Random.Range(-0.1f, 0.1f);
    //     float y =  Random.Range(0.2f, 1f);
    //     float z =  Random.Range(-0.1f, 0.1f);

    //     return new Vector3(x, y, z);
    // }
}

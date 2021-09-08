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
    [Header("Debug")]
    public TMP_Text debugText;

    public GameObject stateManagerContainer;
    StateManager stateManager;

    // Start is called before the first frame update
    async void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();

        if(tourPreviewPrefab && scrollView)
        {
            List<Dictionary<string, object>> tours = await stateManager.FetchTours();
            foreach (Dictionary<string, object> tour in tours)
            {
                if(!tour.ContainsKey("Name"))
                {
                    Debug.Log(String.Format("Name not found in dictionary"));
                    continue;
                }
                Debug.Log(String.Format("Name: {0}", tour["Name"]));
                GameObject preview = Instantiate(tourPreviewPrefab) as GameObject;
                preview.SetActive(true);
                preview.GetComponent<TourPreview>().FromDictionary(tour);
                preview.GetComponent<Button>().onClick.AddListener(() => OnPreviewClicked((string)tour["Id"]));
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
            Debug.Log("tourPreviewPrefab and scrollView should be populated");
        }
    }

    public void OnPreviewClicked(string tourId)
    {
        ToursInfo.CurrentTourId = tourId;
        Task task = PrepareAndLaunchTourAsync(tourId);
    }
    async Task PrepareAndLaunchTourAsync(string tourId)
    {
        Debug.Log(String.Format("PrepareAndLaunchTourAsync: tourId {0}", tourId));
        List<Dictionary<string, object>> locations = await stateManager.FetchTourLocations(tourId);
        ToursInfo.CurrentTourLocations = locations;
        ToursInfo.CurrentLocationIndex = 0;
        Dictionary<string, object> location = ToursInfo.Location();
        Dictionary<string, object> locationData = await stateManager.GetLocationById((string)location["Id"]);
        List<object> scenes = (List<object>)locationData["scenes"];
        Debug.Log(String.Format("PrepareAndLaunchTourAsync {0} scenes in location {1}", scenes.Count, (string)location["Id"]));
        if(scenes.Count > 0)
        {
            Dictionary<string, object> sceneData = await stateManager.FetchScene(scenes[0]);
            Debug.Log((string)sceneData["type"]);
            LoadNextScene(sceneData);
        }
    }

    private void LoadNextScene(Dictionary<string, object> scene)
    {
        ToursInfo.CurrentScene = scene;
        switch (scene["type"])
        {
            case "360Video":
                Loader.LoadVideo360Scene();
                break;
            case "360Gallery":
                Loader.LoadGalleryPanoramicScene();
                break;
            case "VRMap":
                Loader.LoadMapScene();
                break;
            case "Video":
                Debug.LogError("Video Scene is not yet implemented");
                break;
            default:
                break;
        }
        return;
    }

    public async void PopulateTours()
    {
        List<Dictionary<string, object>> tours = await stateManager.FetchTours();
        Debug.Log(String.Format("Number of tours {0}", tours.Count));
        foreach (Dictionary<string, object> tour in tours)
        {
            Debug.Log(String.Format("Name: {0}", tour["Name"]));
        }
    }

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

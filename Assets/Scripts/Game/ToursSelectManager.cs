using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ToursSelectManager : MonoBehaviour
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
        Debug.Log(tourId);
        ToursInfo.CurrentTourId = tourId;
        Task task = PrepareAndLaunchTourAsync(tourId);
    }
    async Task PrepareAndLaunchTourAsync(string tourId)
    {
        List<Dictionary<string, object>> locations = await stateManager.FetchTourLocations(tourId);
        ToursInfo.CurrentTourLocations = locations;
        ToursInfo.CurrentLocationIndex = 0;
        Loader.LoadAugmentedScene();
    }

    // Update is called once per frame
    void Update()
    {

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

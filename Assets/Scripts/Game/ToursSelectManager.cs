using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;
using System;

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
    void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();

        if(tourPreviewPrefab && scrollView)
        {
            for (int i = 0; i < 5; i++)
            {
                // Works with ToursList as scrollView
                // GameObject preview = Instantiate(tourPreviewPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                GameObject preview = Instantiate(tourPreviewPrefab) as GameObject;
                //buttonScript.onClick.AddListener(() => {panel.RecieveButtonInput(index);});
                string buttonID = String.Format("Preview {0} Clicked", i);
                preview.GetComponent<Button>().onClick.AddListener(() => OnPreviewClicked(buttonID));
                TMP_Text title = preview.transform.Find("Title").GetComponent<TMP_Text>();
                if(title != null)
                {
                    title.text = String.Format("Preview {0}", i);
                }
                else
                {
                    debugText.text = "No Title";
                }
                preview.transform.SetParent(scrollView.transform, false);

            }
        }
        else
        {
            Debug.Log("tourPreviewPrefab and scrollView should be populated");
        }
    }

    public void OnPreviewClicked(string name)
    {
        Debug.Log(name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void  PopulateTours()
    {
        Debug.Log(stateManager.Greet());
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

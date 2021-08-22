using System;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class ToursSelectManager : MonoBehaviour
{
    [Header("Preview")]
    public GameObject tourPreviewPrefab;
    public GameObject scrollView;
    [Header("Debug")]
    public TMP_Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        if(tourPreviewPrefab)
        {
            for (int i = 0; i < 5; i++)
            {
                // Works with ToursList as scrollView
                //GameObject preview = Instantiate(tourPreviewPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                GameObject preview = Instantiate(tourPreviewPrefab) as GameObject;
                TMP_Text title = preview.transform.Find("Title").GetComponent<TMP_Text>();
                if(title != null)
                {
                    title.text = String.Format("Hello {0} Paris", i);
                }
                else
                {
                    debugText.text = "No Title";
                }
                preview.transform.SetParent(scrollView.transform, false);

            }


            // float size = preview.GetComponent<Renderer> ().bounds.size.y;
            // Vector3 rescale = preview.transform.localScale;
            // rescale.y = 150 * rescale.y / size;
            // rescale.x = 300 * rescale.x / size;
            // preview.transform.localScale = rescale;

        }
        else
        {
            Debug.Log("No tourPreviewPrefab!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        // GameObject preview = Instantiate(tourPreviewPrefab, new Vector3(0,0,0), Quaternion.identity);
        // preview.GetComponent<Rigidbody>().velocity = getRandomForce();
    }

    private Vector3 getRandomForce()
    {
        float x =  Random.Range(-0.1f, 0.1f);
        float y =  Random.Range(0.2f, 1f);
        float z =  Random.Range(-0.1f, 0.1f);

        return new Vector3(x, y, z);
    }
}

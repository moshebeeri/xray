using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Loader : MonoBehaviour
{
    public Button go = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public static void LoadVideo360Scene()
    {
        SceneManager.LoadScene("Video360Scene");
    }

    public static void LoadMapScene()
    {
        PlayerPrefs.SetFloat("Lat", 48.85885000297237f);
        PlayerPrefs.SetFloat("Lng", 2.2943566789180916f);
        SceneManager.LoadScene("LocationMapScene");
    }

    public static void LoadToursSelectScene()
    {
        SceneManager.LoadScene("TourSelectScene");
    }

    public static void LoadGalleryPanoramicScene()
    {
        SceneManager.LoadScene("GalleryPanoramicScene");
    }

    public static void LoadVideoFlightScene()
    {
        SceneManager.LoadScene("VideoFlightScene");
    }

    public static void LoadSkyCinemaScene()
    {
        SceneManager.LoadScene("SkyCinemaScene");
    }

    public static void LoadScene(Dictionary<string, object> scene)
    {
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
            case "VideoFlight":
                Loader.LoadVideoFlightScene();
                break;
            case "SkyCinema":
                Loader.LoadSkyCinemaScene();
                break;


            default:
                break;
        }
    }
}

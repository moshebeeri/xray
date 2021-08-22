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
        SceneManager.LoadScene(0);
    }

    public static void LoadAugmentedScene()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadMapScene()
    {
        PlayerPrefs.SetFloat("Lat", 48.85885000297237f);
        PlayerPrefs.SetFloat("Lng", 2.2943566789180916f);
        SceneManager.LoadScene(2);
    }

    public static void LoadToursSelectScene()
    {
        SceneManager.LoadScene("TourSelectScene");
    }
}

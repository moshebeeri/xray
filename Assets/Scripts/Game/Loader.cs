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
        SceneManager.LoadScene(2);
    }
}

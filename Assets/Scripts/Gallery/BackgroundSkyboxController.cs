// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BackgroundSkyboxController : MonoBehaviour
{
    string main_scene = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0016.JPG?alt=media&token=c0412b8b-a107-429f-9ddb-0f9bdd63c619";
    string tour_select = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FJerusalem%2FGS__0054.JPG?alt=media&token=286199de-215f-434a-925e-a6a9959629a3";

    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
        string imageUrl = main_scene;
        switch (scene)
        {
            case "MainScene":
                imageUrl = main_scene;
                break;
            case "TourSelectScene":
                imageUrl = tour_select;
                break;
        }

        StartCoroutine(setImage(imageUrl));
    }

    IEnumerator setImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        RenderSettings.skybox.mainTexture = DownloadHandlerTexture.GetContent(request);
    }
}


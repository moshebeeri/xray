// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkyboxPanoramicController : SceneMonoBehaviour
{
    [Header("Next Prev Buttons auto hide")]
    public GameObject NextButton;
    public GameObject PrevButton;
    protected bool auto = true;
    Dictionary<string, object> scene = null;
    List<string> pictures;
    int index = 0;

    FadeUtils controllerUtils = new FadeUtils();
    public void next()
    {
        Debug.Log("next panoramic image");
        if(pictures.Count > index+1)
        {
            index++;
            StartCoroutine(setImage());
        }

    }
    public void prev()
    {
        Debug.Log("prev panoramic image");
        if(index-1 >= 0)
        {
            index--;
            StartCoroutine(setImage());
        }
    }

    private void HandleBoundaries()
    {
        bool has_prev = true;
        bool has_next = true;
        if(index == 0)
            has_prev = false;
        if(index == pictures.Count-1)
            has_next = false;
        NextButton.SetActive(has_next);
        PrevButton.SetActive(has_prev);

    }

    new void Start()
    {
        base.Start();
        scene = ToursInfo.CurrentSceneData;
        if (!scene.ContainsKey("pictures"))
        {
            Debug.LogError("!scene.ContainsKey(pictures)");
            return;
        }
        List<object> object_pictures = (List<object>)scene["pictures"];
        pictures = new List<string>();
        foreach(object o in object_pictures)
            pictures.Add((string)o);
        StartCoroutine(setImage());
    }

    private void onExposureUpdate(float value)
    {
        RenderSettings.skybox.SetFloat("_Exposure", value);
    }

    IEnumerator setImage()
    {
        string url = pictures[index];
        if (CacheUtils.IsCached(url, "JPG"))
            url = "file://" + CacheUtils.fileForUrl(pictures[index], "JPG");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        HandleBoundaries();
        Texture texture = DownloadHandlerTexture.GetContent(request);
        // Fade out
        float startFade = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(controllerUtils.Interpolate(0.15f, startFade, 0.0f, onExposureUpdate));
        // TODO: you can keep user rotation like so: TODO: add ability to rotate, keep and restore rotation
        // RenderSettings.skybox.SetFloat("_Rotation", environment.m_worldRotation)

        // Set Texture
        RenderSettings.skybox.mainTexture = texture;
        // Fade in
        startFade = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(controllerUtils.Interpolate(0.15f, startFade, 1.0f, onExposureUpdate));
        yield return Timer(OnNextPic);
    }
    void OnNextPic(float timePassed)
    {
        Debug.Log(String.Format("+++++ On Next Pic after {0} sec", timePassed));
        if (sceneController && index == pictures.Count - 1)
        {
            Debug.Log("+++++ sceneController.NextScene");
            sceneController.NextScene();
        }
        else
        {
            Debug.Log("+++++ next");
            next();
        }
    }

    //did not worked
    //see: https://forum.unity.com/threads/changing-skybox-material-at-runtime.547177/
    // void setCachedImage()
    // {
    //     string file = CacheUtils.fileForUrl(pictures[index], "JPG");
    //     Texture texture = Resources.Load<Texture>( file );
    //     HandleBoundaries();
    //     RenderSettings.skybox.mainTexture = texture;
    // }
}
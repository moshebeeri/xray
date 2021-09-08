// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SkyboxPanoramicController : MonoBehaviour
{
    // string first_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGP__0060.JPG?alt=media&token=fe975ef1-6ce6-49d0-8765-e96950e84c31";
    // string next_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGP__0019.JPG?alt=media&token=9c9e6bf3-aa08-4b76-9589-66367bac84b1";
    string first_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0016.JPG?alt=media&token=c0412b8b-a107-429f-9ddb-0f9bdd63c619";
    string next_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0023.JPG?alt=media&token=2c23e798-ddd6-4cdf-9492-5e7da180fea0";

    public void next()
    {
        Debug.Log("new panoramic image");
        StartCoroutine(setImage(next_url));
    }

    void Start()
    {
        // start Coroutine to handle the WWW asynchronous process
        StartCoroutine(setImage(first_url));
    }

    IEnumerator setImage(string url)
    {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        //Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        RenderSettings.skybox.mainTexture = DownloadHandlerTexture.GetContent(request);
    }
}
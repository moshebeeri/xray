using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TourPreview : MonoBehaviour
{
    public TMP_Text title;
    public Image thumb;
    public TMP_Text country;
    public TMP_Text city;

  // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FromDictionary(Dictionary<string, object> tour)
    {
        if(tour.ContainsKey("City"))
            city.text = (string)tour["City"];
        if(tour.ContainsKey("Country"))
            country.text = (string)tour["Country"];
        if(tour.ContainsKey("Name"))
            title.text = (string)tour["Name"];
        if (tour.ContainsKey("Thumbnail"))
        {
            Debug.Log((string)tour["Thumbnail"]);
            StartCoroutine(DownloadImage((string)tour["Thumbnail"]));
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if( request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
        {
            Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            thumb.overrideSprite = sprite;
        }
    }

}

// see https://thirteenov.ciihuy.com/how-to-load-a-video-file-from-a-url-and-play-it-in-unity/
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;

public class OnlineVideoLoader : MonoBehaviour
{

    public GameObject VideoPlayerGO;
    public VideoPlayer videoPlayer;
    private string currentVideo = "current.mp4";
    private string previousVideo = "previous.mp4";
    private string nextVideo = "next.mp4";
    private int currentIndex = -1;
    private List<Dictionary<string, object>> locations = new List<Dictionary<string, object>>();

    void Awake()
    {
        videoPlayer = VideoPlayerGO.GetComponent<VideoPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // videoPlayer.url = videoUrl;
        // videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        // videoPlayer.EnableAudioTrack (0, true);
        // videoPlayer.Prepare ();
    }

    // Update is called once per frame
    private IEnumerator DownloadVideo(string url, string filename){
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        Debug.Log ("Downloading!");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
            File.WriteAllBytes(file(filename), www.downloadHandler.data);
            Debug.Log ("File Saved!");
        }
    }
    private string file(string name)
    {
        return Application.persistentDataPath + "/" + name;
    }

    private IEnumerator DownloadNext(string url)
    {
        yield return DownloadVideo(url, nextVideo);
    }
    private IEnumerator DownloadCurrent(string url)
    {
        yield return DownloadVideo(url, currentVideo);
    }
    private IEnumerator DownloadPrevious(string url)
    {
        yield return DownloadVideo(url, previousVideo);
    }


    public IEnumerator StartTour(List<Dictionary<string, object>> locations)
    {
        this.locations = locations;
        if(locations.Count < 1)
        {
            yield return null;
        }
        else
        {
            Dictionary<string, object> start = locations[0];
            yield return DownloadCurrent((string)start["scene_url"]);
            currentIndex = 0;
            videoPlayer.url = file(currentVideo);
            videoPlayer.Play();
            if(locations.Count > currentIndex + 1)
            {
                Dictionary<string, object> next = locations[currentIndex + 1];
                yield return DownloadNext((string)next["scene_url"]);
            }
        }
    }

    public IEnumerator Next()
    {
        File.Delete(file(previousVideo));
        File.Move(file(currentVideo),file(previousVideo));
        File.Move(file(nextVideo), file(currentVideo));
        currentIndex = currentIndex + 1;
        videoPlayer.url = file(currentVideo);
        videoPlayer.Play();
        if(locations.Count > currentIndex + 1)
        {
            Dictionary<string, object> next = locations[currentIndex + 1];
            yield return DownloadNext((string)next["scene_url"]);
        }
    }

    public IEnumerator Prev()
    {
        File.Delete(file(nextVideo));
        File.Move(file(currentVideo),file(nextVideo));
        File.Move(file(previousVideo), file(currentVideo));
        currentIndex = currentIndex - 1;
        videoPlayer.url = file(currentVideo);
        videoPlayer.Play();
        if(currentIndex > 0 )
        {
            Dictionary<string, object> prev = locations[currentIndex + 1];
            yield return DownloadPrevious((string)prev["scene_url"]);
        }
    }

    void PlayVideo(string name)
    {
        videoPlayer.url = file(name);
        videoPlayer.Play();
    }


}

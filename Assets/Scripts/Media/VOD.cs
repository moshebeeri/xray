
using UnityEngine;
using UnityEngine.Video;

public class VOD : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    RenderTexture rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        rt.antiAliasing = 2;
        rt.Create();
        videoPlayer = new VideoPlayer();
        videoPlayer.targetTexture = rt;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

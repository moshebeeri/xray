using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

public class VideoManager : MonoBehaviour
{
    public List<VideoClip> videos = null;
    public VideoPlayer videoPlayer = null;

    private bool isPaused = false;
    public bool IsPaused
    {
        get { return isPaused; }
        private set
        {
            isPaused = value;
            //event
        }
    }

    private bool isReady = false;
    public bool IsReady
    {
        get { return isReady; }
        private set
        {
            isReady = value;
            //event
        }
    }

    private int index = 0;

  // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.seekCompleted += OnComplete;
        videoPlayer.prepareCompleted += OnComplete;
        videoPlayer.loopPointReached += OnLoop;
    }
    public void PauseToggle()
    {
        Debug.Log("PauseToggle");
        if(videoPlayer == null)
        {
            Debug.Log("No Video Player");
            return;
        }
        IsPaused = !videoPlayer.isPaused;
        if (IsPaused)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    public void Play()
    {
        videoPlayer.Play();
    }
    public void Stop()
    {
        videoPlayer.Stop();
    }
    public void pause()
    {
        videoPlayer.Pause();
    }
    public void url(string link)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = link;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += PrepareCompleted;
    }

    private void PrepareCompleted(VideoPlayer source)
    {
        Play();
    }

    private void OnDestroy() {
        videoPlayer.seekCompleted -= OnComplete;
        videoPlayer.prepareCompleted -= OnComplete;
        videoPlayer.loopPointReached -= OnLoop;
    }

    public void SeekForward()
    {
        Seek(10f);
    }
    public void SeekBack()
    {
        Seek(-10f);
    }
    public void Seek(float seconds)
    {
        IsReady = false;
        videoPlayer.time += seconds;
    }
    public void Next()
    {
        index = (index+1) % videos.Count ;
        Prepare(index);
    }
    public void Prev()
    {
        index = index == 0? 0 :  videos.Count-1;
        Prepare(index);
    }
    private void Prepare(int clipIndex)
    {
        IsReady = false;
        videoPlayer.clip = videos[clipIndex];
        videoPlayer.Prepare();
    }

    private void OnComplete(VideoPlayer videoPlayer)
    {
        IsReady = true;
        videoPlayer.Play();
    }
    private void OnLoop(VideoPlayer videoPlayer)
    {
        //Next();
    }
}


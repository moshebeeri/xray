using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SceneController : MonoBehaviour
{
    [Header("StateManager")]
    public GameObject stateManagerContainer;
    protected StateManager stateManager;

    [Header("Auto Hide")]
    public GameObject nextScene;
    public GameObject prevScene;
    public GameObject nextLocation;
    public GameObject prevLocation;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();
        PrepareNextSceneCache();
    }

    protected Dictionary<string, object> CurrnetSceneData()
    {
        return ToursInfo.CurrentSceneData;
    }
    async protected Task<Dictionary<string, object>> NextSceneData()
    {
        object next = ToursInfo.NextSceneRef();
        if (next != null)
        {
            Dictionary<string, object> nextSceneData = await stateManager.FetchScene(next);
            ToursInfo.NextSceneData = nextSceneData;
            return nextSceneData;
        }
        return null;
    }
    void Update()
    {
        AutoHide();
    }
    private void AutoHide()
    {
        if(ToursInfo.CurrentLocation == null)
            return;
        bool showNextScene = false;
        bool showPrevScene = false;
        bool showNextLocation = false;
        bool showPrevLocation = false;

        bool LastLocation = ToursInfo.TourLocations.Count==0 || ToursInfo.CurrentLocationIndex == ToursInfo.TourLocations.Count - 1;
        bool LastScene = ToursInfo.CurrentSceneIndex == ((List<object>)ToursInfo.CurrentLocation["scenes"]).Count - 1;

        if( LastLocation && LastScene)
        {
            Debug.Log(String.Format("End of tour"));
        }else{
            showNextScene = ToursInfo.CurrentSceneIndex <  ((List<object>)ToursInfo.CurrentLocation["scenes"]).Count - 1;
            showPrevScene = ToursInfo.CurrentSceneIndex > 0;
            showNextLocation = !showNextScene;
            showPrevLocation = !showPrevScene;

        }
        nextScene.SetActive(showNextScene);
        prevScene.SetActive(showPrevScene);
        nextLocation.SetActive(showNextLocation);
        prevLocation.SetActive(showPrevLocation);
    }

    async protected Task<Dictionary<string, object>> PreviousSceneData()
    {
        object prev = ToursInfo.PrevSceneRef();
        if(prev != null)
        {
            Dictionary<string, object> prevSceneData = await stateManager.FetchScene(prev);
            ToursInfo.PreviousSceneData = prevSceneData;
            return prevSceneData;
        }
        return null;
    }

    public async void NextScene()
    {
		Debug.Log("NextScene");
		// 1. prev<-curr<-next
		// 2. load next Assets and set it current
		// 3. delete prev scene assets
		object next = ToursInfo.NextScene();
        if(next == null)
            return;
        ToursInfo.NextSceneData = await stateManager.FetchScene(next);
        if(ToursInfo.PreviousSceneData != null)
            DeleteSceneData(ToursInfo.PreviousSceneData);
        FetchSceneData(ToursInfo.NextSceneData);
		ToursInfo.PreviousSceneData = ToursInfo.CurrentSceneData;
        ToursInfo.CurrentSceneData = ToursInfo.NextSceneData;
        ToursInfo.NextSceneData = null;
		PrepareNextSceneCache();
        ToursInfo.LogState();
        Loader.LoadScene(ToursInfo.CurrentSceneData);
    }
    //TODO: Add next location first scene
    async void PrepareNextSceneCache(){
        object nextRef = ToursInfo.NextSceneRef();
        if(nextRef == null)
            return;
        Dictionary<string, object> SceneData = await stateManager.FetchScene(nextRef);
		FetchSceneData(SceneData);
    }
    //TODO: Add prev location first scene
    async void PreparePrevCache(){
        object prevRef = ToursInfo.PrevSceneRef();
        if(prevRef == null)
            return;
        Dictionary<string, object> SceneData = await stateManager.FetchScene(prevRef);
		FetchSceneData(SceneData);
    }
    public async void PreviousScene()
    {
		Debug.Log("PreviousScene");
        // 1. prev->curr->next
        // 2. load prev Assets
		// 3. delete next scene assets
        object prev = ToursInfo.PrevScene();
		if (prev == null)
		{
            Debug.Log("PreviousScene called with null prev");
			return;
		}

        ToursInfo.PreviousSceneData = await stateManager.FetchScene(prev);
        if(ToursInfo.NextSceneData != null)
            DeleteSceneData(ToursInfo.NextSceneData);
		FetchSceneData(ToursInfo.PreviousSceneData);

		ToursInfo.NextSceneData = ToursInfo.CurrentSceneData;
		ToursInfo.CurrentSceneData = ToursInfo.PreviousSceneData;
        ToursInfo.PreviousSceneData = null;
        PreparePrevCache();
		ToursInfo.LogState();
        Loader.LoadScene(ToursInfo.CurrentSceneData);
    }
    private async void GetLocation(object locationRef, bool isNext)
    {
        Dictionary<string, object> locationData = await stateManager.FetchLocation(locationRef);
        ToursInfo.CurrentLocation = locationData;
        List<object> scenes = (List<object>)locationData["scenes"];
        Debug.Log(String.Format("SceneController GetLocation - {0} scenes in location {1}", scenes.Count, locationData["Name"]));
        if(scenes.Count > 0)
        {
            Dictionary<string, object> sceneData = await stateManager.FetchScene(scenes[0]);
            if(isNext)
            {
                if(ToursInfo.PreviousSceneData != null)
                    DeleteSceneData(ToursInfo.PreviousSceneData);
                ToursInfo.PreviousSceneData = ToursInfo.CurrentSceneData;
            }else{
                if(ToursInfo.NextSceneData != null)
                    DeleteSceneData(ToursInfo.NextSceneData);
                ToursInfo.NextSceneData = ToursInfo.CurrentSceneData;
            }
            ToursInfo.CurrentSceneData = sceneData;
            ToursInfo.CurrentSceneIndex = 0;
            Loader.LoadScene(sceneData);
        }
    }

    public void NextLocation()
	{
		Debug.Log("NextLocation");
        Dictionary<string, object> next = ToursInfo.NextLocation();
        if(next != null)
            GetLocation(next["Location"], true);
        ToursInfo.LogState();
	}
	public void PrevLocation()
    {
        Debug.Log("PrevLocation");
        Dictionary<string, object> prev = ToursInfo.PrevLocation();
        if(prev != null)
            GetLocation(prev["Location"], false);
        ToursInfo.LogState();
    }

	public void MainMenu()
    {
        Debug.Log("MainMenu");
        ToursInfo.LogState();
        ResetState();
        Loader.LoadMainScene();
    }

	public void TourSelect()
    {
        Debug.Log("TourSelect");
        ToursInfo.LogState();
        ResetState();
        Loader.LoadToursSelectScene();
    }

    public void ResetState()
    {
        DeleteSceneData(ToursInfo.CurrentSceneData);
        DeleteSceneData(ToursInfo.PreviousSceneData);
        DeleteSceneData(ToursInfo.NextSceneData);
        ToursInfo.ResetState();
    }

    protected void DeleteSceneData(Dictionary<string, object> scene)
    {
        if(scene == null)
            return;
        if((string)scene["type"] == "360Video")
        {
            CacheUtils.RemoveCacheVideo(scene);
        }
	}
    protected void FetchSceneData(Dictionary<string, object> scene)
    {
        if(scene != null && (string)scene["type"] == "360Video")
        {
            StartCoroutine(FetchAndCacheVideo(scene));
        }
    }

    public IEnumerator FetchAndCacheGallery(Dictionary<string, object> scene)
    {
        if(scene == null || !scene.ContainsKey("pictures"))
        {
            Debug.LogError("scene == null || !scene.ContainsKey(pictures)");
            yield break;
        }
        List<object> object_pictures = (List<object>)scene["pictures"];

        foreach (object pic_obj in object_pictures)
        {
            string pic_url = (string)pic_obj;
            string filename = CacheUtils.fileForUrl(pic_url, "JPG");
            if (!File.Exists(filename))
            {
                UnityWebRequest www = new UnityWebRequest(pic_url);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else{
                    File.WriteAllBytes(filename, www.downloadHandler.data);
                }
            }
        }
    }
    public IEnumerator FetchAndCacheVideo(Dictionary<string, object> scene)
    {
        if(scene == null || !scene.ContainsKey("url"))
        {
            Debug.LogError("scene == null || !scene.ContainsKey(url)");
            yield break;
        }
        string url = (string)scene["url"];
        string extension = "mp4";
        StartCoroutine(FetchAndCacheURL(url, extension));
    }
    public IEnumerator FetchAndCacheURL(string url, string extension)
    {
        string filename = CacheUtils.fileForUrl(url, extension);
        if (!File.Exists(CacheUtils.file(filename)))
        {
            UnityWebRequest www = new UnityWebRequest(url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                yield break;
            }
            // Or retrieve results as binary data
            File.WriteAllBytes(filename, www.downloadHandler.data);
        }
    }




}

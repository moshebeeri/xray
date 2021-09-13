using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{    [Header("StateManager")]
    public GameObject stateManagerContainer;
    protected StateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = stateManagerContainer.GetComponent<StateManager>();
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
            StartCoroutine(DeleteSceneData(ToursInfo.PreviousSceneData));
		StartCoroutine(FetchSceneData(ToursInfo.NextSceneData));
		ToursInfo.PreviousSceneData = ToursInfo.CurrentSceneData;
        ToursInfo.CurrentSceneData = ToursInfo.NextSceneData;
        ToursInfo.NextSceneData = null;
		ToursInfo.LogState();
        Loader.LoadScene(ToursInfo.CurrentSceneData);
        //TODO: prepare next cache
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
            StartCoroutine(DeleteSceneData(ToursInfo.NextSceneData));
		StartCoroutine(FetchSceneData(ToursInfo.PreviousSceneData));

		ToursInfo.NextSceneData = ToursInfo.CurrentSceneData;
		ToursInfo.CurrentSceneData = ToursInfo.PreviousSceneData;
        ToursInfo.PreviousSceneData = null;
		ToursInfo.LogState();
        Loader.LoadScene(ToursInfo.CurrentSceneData);
        //TODO: prepare prev cache
    }
    private async void GetLocation(string locationId)
    {
        Dictionary<string, object> locationData = await stateManager.GetLocationById(locationId);
        ToursInfo.CurrentLocation = locationData;
        List<object> scenes = (List<object>)locationData["scenes"];
        Debug.Log(String.Format("SceneController GetLocation - {0} scenes in location {1}", scenes.Count, locationId));
        if(scenes.Count > 0)
        {
            Dictionary<string, object> sceneData = await stateManager.FetchScene(scenes[0]);
            ToursInfo.CurrentSceneData = sceneData;
            Loader.LoadScene(sceneData);
        }

    }
    public void NextLocation()
	{
		Debug.Log("NextLocation");
        Dictionary<string, object> next = ToursInfo.NextLocation();
        if(next != null)
            GetLocation((string)next["Id"]);

        ToursInfo.LogState();
	}
	public void PrevLocation()
    {
        Debug.Log("PrevLocation");
        Dictionary<string, object> prev = ToursInfo.PrevLocation();
        if(prev != null)
            GetLocation((string)prev["Id"]);
        ToursInfo.LogState();
    }

	public void MainMenu()
    {
        Debug.Log("MainMenu");
        ToursInfo.LogState();
        Loader.LoadMainScene();
    }

	public void TourSelect()
    {
        Debug.Log("TourSelect");
        ToursInfo.LogState();
        Loader.LoadToursSelectScene();
    }

    protected IEnumerator DeleteSceneData(Dictionary<string, object> scene)
    {
        if((string)scene["type"] == "Video360")
        {
            URLFileHandler handler = new URLFileHandler("url");
            StartCoroutine(handler.RemoveLocalData(scene));
        }
		yield break;
	}

    protected IEnumerator FetchSceneData(Dictionary<string, object> scene)
    {
        if((string)scene["type"] == "Video360")
        {
            URLFileHandler handler = new URLFileHandler("url");
            handler.Fetch(stateManager, scene);
        }
        yield break;
    }
}

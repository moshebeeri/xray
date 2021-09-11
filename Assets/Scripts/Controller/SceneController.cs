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
		object next = ToursInfo.NextSceneRef();
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
    }

    public async void PreviousScene()
    {
		Debug.Log("PreviousScene");
        // 1. prev->curr->next
        // 2. load prev Assets
		// 3. delete next scene assets
        Dictionary<string, object> next = ToursInfo.NextSceneData;
		if (next == null)
		{
            Debug.Log("PreviousScene called with null next");
			return;
		}
		ToursInfo.NextSceneData = ToursInfo.CurrentSceneData;
		ToursInfo.CurrentSceneData = ToursInfo.PreviousSceneData;
		object prev = ToursInfo.PrevScene();
		ToursInfo.PreviousSceneData = await stateManager.FetchScene(prev);
		StartCoroutine(DeleteSceneData(next));
		StartCoroutine(FetchSceneData(ToursInfo.PreviousSceneData));
		ToursInfo.LogState();
    }

	public void NextLocation()
	{
		Debug.Log("NextLocation");
        ToursInfo.LogState();

	}
	public void PrevLocation()
    {
        Debug.Log("PrevLocation");
        ToursInfo.LogState();
    }

	public void MainMenu()
    {
        Debug.Log("MainMenu");
        ToursInfo.LogState();
    }

	public void TourSelect()
    {
        Debug.Log("TourSelect");
        ToursInfo.LogState();
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
            StartCoroutine(handler.Fetch(stateManager, scene));
        }
        yield break;
    }
}

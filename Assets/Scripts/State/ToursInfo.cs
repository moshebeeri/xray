using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ToursInfo {

    public static string CurrentTourId { get; set; }
	public static Dictionary<string, Dictionary<string, object>> Tours { get; set; } = new Dictionary<string, Dictionary<string, object>>();
	public static Dictionary<string, object> CurrentTour { get; set; }
    public static List<Dictionary<string, object>> TourLocations { get; set; }
    public static Dictionary<string, object> CurrentLocation { get; set; }
    public static Dictionary<string, object> PreviousSceneData { get; set; }
    public static Dictionary<string, object> CurrentSceneData { get; set; }
    public static Dictionary<string, object> NextSceneData { get; set; }
    public static int CurrentLocationIndex { get; set; } = 0;
    public static int CurrentSceneIndex { get; set; } = 0;

    public static Dictionary<string, object> Location()
    {
        return TourLocations[CurrentLocationIndex];
    }

    public static object SceneRef()
    {
        Dictionary<string, object> location = CurrentLocation;
        return ((List<object>)location["scenes"])[CurrentSceneIndex];
    }
    public static object NextSceneRef()
    {
        Dictionary<string, object> location = CurrentLocation;
        if(location == null)
        {
            Debug.LogError("location == null");
            return null;
        }
		List<object> scenes = (List<object>)location["scenes"];
        if(scenes.Count <= CurrentSceneIndex+1)
        {
            return null;
        }
        Debug.Log(String.Format("NextSceneRef Return scene index {0}",CurrentSceneIndex+1));
        return ((List<object>)location["scenes"])[CurrentSceneIndex+1];
    }
    public static object PrevSceneRef()
    {
        Dictionary<string, object> location = CurrentLocation;
        if(location == null)
			return null;
        List<object> scenes = (List<object>)location["scenes"];
        if(CurrentSceneIndex <= 0)
            return null;
        return ((List<object>)location["scenes"])[CurrentSceneIndex-1];
    }

    public static Dictionary<string, object> NextLocation()
    {
        if(TourLocations.Count > CurrentLocationIndex+1)
        {
            CurrentLocationIndex++;
            return TourLocations[CurrentLocationIndex];
        }
        return null;
    }

    public static Dictionary<string, object> PrevLocation()
    {
        if(CurrentLocationIndex>0)
        {
            CurrentLocationIndex--;
            return TourLocations[CurrentLocationIndex];
        }
        return null;
    }

    public static object NextScene()
    {
        object s = NextSceneRef();
        if(s != null)
            CurrentSceneIndex++;
        return s;
    }

    public static object PrevScene()
    {
        object s = PrevSceneRef();
        if(s != null)
            CurrentSceneIndex--;
        return s;
    }

    public static void LogState()
    {
        Debug.Log( "------------Start LogState--------------" );
        if(CurrentTour != null)
            Debug.Log( String.Format("Tour: {0}", CurrentTour["Name"]) );
        if(CurrentLocation != null)
            Debug.Log( String.Format("Location: {0}", CurrentLocation["Name"]) );
        Debug.Log( String.Format("CurrentSceneIndex: {0}", CurrentSceneIndex) );
        if(CurrentSceneData != null)
        {
            Debug.Log( String.Format("Scene name: {0}", CurrentSceneData["name"]));
            Debug.Log( String.Format("Scene type: {0}", CurrentSceneData["type"]));
        }
        if(NextSceneData != null)
            Debug.Log( String.Format("Next Scene: {0}", NextSceneData["name"]));
        if(PreviousSceneData != null)
        {
            if(PreviousSceneData.ContainsKey("name"))
                Debug.Log( String.Format("Prev Scene: {0}", PreviousSceneData["name"]));
            else
                Debug.LogError("NOT PreviousSceneData.ContainsKey name");
        }

        Debug.Log( "------------End LogState--------------" );
	}
}
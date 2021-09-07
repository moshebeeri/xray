using System.Collections.Generic;
public class ToursInfo {

    public static string CurrentTourId { get; set; }
    public static List<Dictionary<string, object>> CurrentTourLocations { get; set; }
    public static Dictionary<string, object> CurrentLocation { get; set; }
    public static Dictionary<string, object> CurrentScene { get; set; }
    public static int CurrentLocationIndex { get; set; } = 0;
    public static int CurrentSceneIndex { get; set; } = 0;

    public static Dictionary<string, object> Location()
    {
        return ToursInfo.CurrentTourLocations[ToursInfo.CurrentLocationIndex];
    }

    public static object SceneRef()
    {
        Dictionary<string, object> location = Location();
        return ((List<object>)location["scenes"])[CurrentSceneIndex];
    }

    public static Dictionary<string, object> NextLocation()
    {
        return null;
    }

    public static Dictionary<string, object> NextScene()
    {
        return null;
    }
}
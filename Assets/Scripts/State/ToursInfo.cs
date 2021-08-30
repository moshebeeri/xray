using System.Collections.Generic;
public class ToursInfo {

    public static string CurrentTourId { get; set; }
    public static List<Dictionary<string, object>> CurrentTourLocations { get; set; }
    public static int CurrentLocationIndex { get; set; } = 0;
}
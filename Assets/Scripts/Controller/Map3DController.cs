using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Examples.Shared;
using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine.UI;

namespace Google.Maps.Examples {
  /// <summary>
  /// This example demonstrates a basic usage of the Maps SDK for Unity.
  /// </summary>
  /// <remarks>
  /// By default, this script loads the Statue of Liberty. If a new lat/lng is set in the Unity
  /// inspector before pressing start, that location will be loaded instead.
  /// </remarks>
    [RequireComponent(typeof(MapsService))]
    public class Map3DController : MonoBehaviour {
        [Tooltip("LatLng to load (must be set before hitting play).")]
        public LatLng LatLng = new LatLng(40.6892199, -74.044601);

        [Header("Location Info")]
        [Tooltip("Text input which info will be presented")]
        public Text info;

        /// <summary>
        /// Use <see cref="MapsService"/> to load geometry.
        /// </summary>
        private void Start() {
        // Get required MapsService component on this GameObject.
            MapsService mapsService = GetComponent<MapsService>();
            Dictionary<string, object> sceneData = ToursInfo.CurrentSceneData;
            //TODO: Hide the dialog incase no info provided.
            string infoText = ToursInfo.CurrentSceneData.ContainsKey("info") ? (string)ToursInfo.CurrentSceneData["info"] : "";
            info.text = infoText;
            GeoPoint gp = (GeoPoint)sceneData["location"];
            float lat = (float)gp.Latitude;// PlayerPrefs.GetFloat("Lat");
            float lng = (float)gp.Longitude; //PlayerPrefs.GetFloat("Lng");
            if(lat > 0 && lng > 0)
            LatLng = new LatLng(lat, lng);

            // Set real-world location to load.
            mapsService.InitFloatingOrigin(LatLng);

            // Register a listener to be notified when the map is loaded.
            mapsService.Events.MapEvents.Loaded.AddListener(OnLoaded);

            // Load map with default options.
            mapsService.LoadMap(ExampleDefaults.DefaultBounds, ExampleDefaults.DefaultGameObjectOptions);
        //mapsService.LoadMap(new Bounds(Vector3.zero, new Vector3(10000, 0, 10000)), ExampleDefaults.DefaultGameObjectOptions);

        }

        /// <summary>
        /// Example of OnLoaded event listener.
        /// </summary>
        /// <remarks>
        /// The communication between the game and the MapsSDK is done through APIs and event listeners.
        /// </remarks>
        public void OnLoaded(MapLoadedArgs args) {
        // The Map is loaded - you can start/resume gameplay from that point.
        // The new geometry is added under the GameObject that has MapsService as a component.
        }
    }
}

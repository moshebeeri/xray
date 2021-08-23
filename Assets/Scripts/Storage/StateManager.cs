using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using System.Threading.Tasks;

public class StateManager : MonoBehaviour
{
    FirebaseFirestore db;
    List<Dictionary<string, object>> tours = new List<Dictionary<string, object>> ();

    [Header("Debug")]
    public TMP_Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.Log("CheckAndFixDependenciesAsync() FAILED: " + task.Exception);
                return;
            }
            db = FirebaseFirestore.DefaultInstance;
        });
    }

    // See: https://stackoverflow.com/questions/32306704/how-to-pass-data-between-scenes-in-unity/44542726#44542726
    public void SetUserInfo(String UserId, String CurrentTour, String CurrentLocation)
    {
        PlayerPrefs.SetString("UserId", UserId);
        PlayerPrefs.SetString("CurrentTour", CurrentTour);
        PlayerPrefs.SetString("CurrentLocation", CurrentLocation);

    }

    public Tourist GetUserInfo()
    {
        Tourist tourist = new Tourist(
            PlayerPrefs.GetString("UserId"),
            PlayerPrefs.GetString("CurrentTour"),
            PlayerPrefs.GetString("CurrentLocation"));
        return tourist;
    }

    public void SaveTouristData(Tourist tourist)
    {
        DocumentReference docRef = db.Collection("tourists").Document(tourist.UserId);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "userId", tourist.UserId },
            { "currentTour", tourist.CurrentTourId},
            { "currentLocation", tourist.CurrentLocationId },
        };
        docRef.SetAsync(data).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to document in tours collection.");
        });
    }

    public void ReadTouristData(Tourist tourist)
    {
        DocumentReference docRef = db.Collection("tourists").Document(tourist.UserId);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "userId", tourist.UserId },
            { "currentTour", tourist.CurrentTourId},
            { "currentLocation", tourist.CurrentLocationId },
        };
        docRef.SetAsync(data).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to document in tours collection.");
        });
    }

    private List<Dictionary<string, object>> Extract(QuerySnapshot snapshot)
    {
        List<Dictionary<string, object>> docs = new List<Dictionary<string, object>> ();
        int i = 0;
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            debugText.text = "try to read";
            Debug.Log(String.Format("Snapshot {0}", i++));

            Dictionary<string, object> tour = document.ToDictionary();
            docs.Add(tour);
        }
        return docs;
    }

    public void GetTours()
    {
        List<Dictionary<string, object>>  all = GetAllTours();
        debugText.text = "Number of tours is " + all.Count;
        // debugText.text = "try to read";
        // ReadData();
    }

    private void ReadData()
    {
        // Read data
        CollectionReference usersRef = db.Collection("tours");
        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                int i = 0;
                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Debug.Log(String.Format("Snapshot {0}", i++));
                    Dictionary<string, object> documentDictionary = document.ToDictionary();
                    Debug.Log(String.Format("Name: {0}", documentDictionary["Name"]));
                }

                Debug.Log("Read all data from the tours collection.");
            });
    }


    public List<Dictionary<string, object>>  GetAllTours()
    {
        // Read data
        CollectionReference toursRef = db.Collection("tours");
        toursRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            tours = Extract(task.Result);
        });
        return null;
    }

    public List<Dictionary<string, object>> GetCitiesByCountry(String Country)
    {
        List<Dictionary<string, object>> cities = new List<Dictionary<string, object>> ();
        CollectionReference citiesRef = db.Collection("cities");
        Query query = citiesRef.WhereEqualTo("Country", Country);
        query.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
        {
            cities = Extract(task.Result);
        });
        return cities;
    }

    public List<Dictionary<string, object>> GetCitiesByState(String State)
    {
        List<Dictionary<string, object>> cities = new List<Dictionary<string, object>> ();
        CollectionReference citiesRef = db.Collection("cities");
        Query query = citiesRef.WhereEqualTo("State", State);
        query.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
        {
            cities = Extract(task.Result);
        });
        return cities;
    }

    public List<Dictionary<string, object>>  GetToursByCity(String City)
    {
        List<Dictionary<string, object>> tours = new List<Dictionary<string, object>> ();
        CollectionReference citiesRef = db.Collection("tours");
        Query query = citiesRef.WhereEqualTo("City", City);
        query.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
        {
            tours = Extract(task.Result);
        });
        return tours;
    }

    public string Greet()
    {
        return "Hi You!!!";
    }

    public async Task<List<Dictionary<string, object>>>  FetchTours()
    {
        // Read data
        CollectionReference toursRef = db.Collection("tours");
        QuerySnapshot snapshot = await toursRef.GetSnapshotAsync();
        List<Dictionary<string, object>> tours = Extract(snapshot);
        return tours;
    }

    // private IEnumerator LoadScoreboardData()
    // {
    //     //Get all the users data ordered by kills amount
    //     var DBTask = DBreference.Child("users").OrderByChild("kills").GetValueAsync();

    //     yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //     if (DBTask.Exception != null)
    //     {
    //         Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //     }
    //     else
    //     {
    //         //Data has been retrieved
    //         DataSnapshot snapshot = DBTask.Result;

    //         //Destroy any existing scoreboard elements
    //         foreach (Transform child in scoreboardContent.transform)
    //         {
    //             Destroy(child.gameObject);
    //         }

    //         //Loop through every users UID
    //         foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
    //         {
    //             string username = childSnapshot.Child("username").Value.ToString();
    //             int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
    //             int deaths = int.Parse(childSnapshot.Child("deaths").Value.ToString());
    //             int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());

    //             //Instantiate new scoreboard elements
    //             GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
    //             scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
    //         }

    //         //Go to scoareboard screen
    //         UIManager.instance.ScoreboardScreen();
    //     }
    // }


}


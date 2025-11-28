using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;

public class LoadDataManager : MonoBehaviour
{
    public static FirebaseUser firebaseUser;
    public static User userInGame;
    public static bool IsUserLoaded { get; private set; }
    private DatabaseReference reference;
    private void Awake()
    {
        firebaseUser = FirebaseAuth.DefaultInstance.CurrentUser;
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getUserInGame(); 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getUserInGame()
    {
        reference.Child("Users").Child(firebaseUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("doc du lieu that bai " + task.Exception);
                IsUserLoaded = false;
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                userInGame = JsonConvert.DeserializeObject<User>(snapshot.Value.ToString());
                IsUserLoaded = true;
            }
          
        });
    }
}

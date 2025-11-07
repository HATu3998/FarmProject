
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FireBaseDatabaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private void Awake()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    private void Start()
    {
        TilemapDetail tilemapDetail = new TilemapDetail(1, 1, TilemapState.Ground);
        writeDatabase("123", tilemapDetail.ToString());
        readDatabase("123");
    }
    public void writeDatabase(string id, string message)
    {
        reference.Child("User").Child(id).SetValueAsync(message).ContinueWithOnMainThread(task
            =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("ghi du lieu thanh cong");
            }
            else
            {
                Debug.Log("ghi du lieu that bai"+task.Exception);
            }
        });
    }
    public void readDatabase(string id)
    {
        reference.Child("User").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("doc du lieu thanh cong "+snapshot.Value.ToString());
                }
                else
                {
                    Debug.Log("doc du lieu that bai"+task.Exception);
                }
        });
    }
}

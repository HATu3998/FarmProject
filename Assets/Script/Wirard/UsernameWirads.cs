using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UsernameWirads : MonoBehaviour
{
    public Text username; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject usernameWizard;
    public InputField ipUsername;
    public Button buttonOK;

    private FireBaseDatabaseManager databaseManager;
    void Start()
    {
        databaseManager = GameObject.Find("DatabaseManager").GetComponent<FireBaseDatabaseManager>();

        usernameWizard.SetActive(false);
        StartCoroutine(WaiForUserData());
        buttonOK.onClick.AddListener(SetNewUserName);
    }

    // Update is called once per frame

    IEnumerator WaiForUserData()
    {
        while (!LoadDataManager.IsUserLoaded)
        {
            yield return null;
        }
        Debug.Log("UsernameWizard: User loaded ,Name=" + LoadDataManager.userInGame.Name);
        if (string.IsNullOrEmpty(LoadDataManager.userInGame.Name) || LoadDataManager.userInGame.Name == ".")
        {
            usernameWizard.SetActive(true);
        }
        else
        {
            username.text = LoadDataManager.userInGame.Name;
        }
    }
    void Update()
    {
        
    }
    public void SetNewUserName()
    {
        if (ipUsername.text != "")
        {
            LoadDataManager.userInGame.Name = ipUsername.text;
            databaseManager.writeDatabase("Users/" + LoadDataManager.firebaseUser.UserId, LoadDataManager.userInGame.ToString());
            usernameWizard.SetActive(false);
            
        }
    }
}

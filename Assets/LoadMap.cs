using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour
{
    public Button buttonLoadDone;
    public string nextSceneName = "SampleScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // L?c m?i v?o, ch?a ch?c ?? load xong user => disable n?t
        buttonLoadDone.interactable = false;

        // ??i ??n khi LoadDataManager b?o ?? load xong
        StartCoroutine(WaitForUserData());

        // Khi b?m n?t, ch? cho v?o scene n?u ?? load xong
        buttonLoadDone.onClick.AddListener(OnClickLoad);
    }
    IEnumerator WaitForUserData()
    {
        while (!LoadDataManager.IsUserLoaded)
        {
            yield return null; // ??i frame sau
        }

        // ?? load xong user => cho ph?p b?m n?t
        buttonLoadDone.interactable = true;
        Debug.Log("User data loaded, you can enter game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickLoad()
    {
        if (!LoadDataManager.IsUserLoaded)
        {
            Debug.LogWarning("User data not loaded yet, please wait");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}

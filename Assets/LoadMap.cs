using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour
{
    public Button buttonLoadDone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadMap()
    {
        buttonLoadDone.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });
    }
}

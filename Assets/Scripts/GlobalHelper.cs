using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalHelper : MonoBehaviour
{
    public GameObject SettingsMenuGO;


    void Start()
    {
        SettingsMenuGO.SetActive(false);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSettingsButton()
    {
        SettingsMenuGO.SetActive(true);
    }

    public void OnResumeButton()
    {
        SettingsMenuGO.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}

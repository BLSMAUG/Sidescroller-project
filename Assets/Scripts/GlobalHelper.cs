using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GlobalHelper : MonoBehaviour
{
    public GameObject SettingsMenuGO;
    public GameObject ExitConfirmGO;

    void Start()
    {
        SettingsMenuGO.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Combat")
        {
            ExitConfirmGO.SetActive(false);
        }
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSettingsButton()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            SettingsMenuGO.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "Combat" && BattleSystem.isChoosingMove == true)
        {
            SettingsMenuGO.SetActive(true);
        }
    }

    public void OnResumeButton()
    {
        SettingsMenuGO.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnMainMenuButton()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene(0);
        }
        else if (SceneManager.GetActiveScene().name == "Combat")
        {
            ExitConfirmGO.SetActive(true);
        }
    }

    public void OnExitConfirmButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitDenyButton()
    {
        ExitConfirmGO.SetActive(false);
    }

    public void PauseMenu(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SettingsMenuGO.SetActive(true);
        }
    }
}

using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GlobalHelper : MonoBehaviour
{
    public GameObject SettingsMenuGO;
    public GameObject ExitConfirmGO;
    public GameObject NewGameButtonGO;
    public GameObject NewGameButtonMaskGO;

    public GameObject PlayerSpawnPointGO;
    public GameObject playerPrefab;

    private bool baseDataSaved = false;
    private bool newGame = false;

    void Start()
    {
        SettingsMenuGO.SetActive(false);

        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            NewGameMask();
        }

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            if (baseDataSaved == true && newGame == false)
            {
                GameObject playerGO = Instantiate(playerPrefab, PlayerSpawnPointGO.transform);
                CameraGround.FindPlayer();
                Chest.FindPlayer();
                Unit.instance.LoadPlayer();
            }
            else if (baseDataSaved == true && newGame == true)
            {
                GameObject playerGO = Instantiate(playerPrefab, PlayerSpawnPointGO.transform);
                CameraGround.FindPlayer();
                Chest.FindPlayer();
                Unit.instance.LoadBasePlayer();
                newGame = false;
            }
            else if (baseDataSaved == false)
            {
                Debug.Log("1");
                GameObject playerGO = Instantiate(playerPrefab, PlayerSpawnPointGO.transform);
                CameraGround.FindPlayer();
                Chest.FindPlayer();
                Debug.Log("2");
                Unit.instance.SaveBasePlayer();
                Unit.instance.SavePlayer();
                Debug.Log("3");
                baseDataSaved = true;
            }

            PlayerMovementNew.instance.GetComponent<CapsuleCollider2D>().enabled = true;
            PlayerMovementNew.instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            PlayerMovementNew.instance.GetComponent<PlayerMovementNew>().enabled = true;
            PlayerMovementNew.instance.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

        }

        if (SceneManager.GetActiveScene().name == "Combat")
        {
            ExitConfirmGO.SetActive(false);
            PlayerMovementNew.instance.GetComponent<CapsuleCollider2D>().enabled = false;
            PlayerMovementNew.instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            PlayerMovementNew.instance.GetComponent<PlayerMovementNew>().enabled = false;
            PlayerMovementNew.instance.GetComponent<Transform>().localScale = new Vector3(0.55f, 0.55f, 0.55f);
        }

    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }


    public void NewGameMask()
    {
        if (baseDataSaved == false)
        {
            Debug.Log("yo");
            NewGameButtonGO.SetActive(false);
            NewGameButtonMaskGO.SetActive(true);
        }
        else
        {
            NewGameButtonGO.SetActive(true);
            NewGameButtonMaskGO.SetActive(false);
        }
    }
    public void OnNewGameButton()
    {
        newGame = true;
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

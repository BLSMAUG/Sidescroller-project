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

    public bool baseDataSaved = false;
    public bool newGame = false;

    //public static GlobalHelper instance;
    public GlobalHelperDataV2 save;

    private void Awake()
    {
        //instance = this;
    }
    void Start()
    {
        SettingsMenuGO.SetActive(false);

        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            //instance.LoadHelper();
            save = SaveGlobalHelperDataV2.Load();
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
                baseDataSaved = true;
                //instance.SaveHelper();
                SaveGlobalHelperDataV2.Save(save);
                Debug.Log("3");
                
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
            PlayerMovementNew.instance.GetComponent<Transform>().localScale = new Vector3(0.65f, 0.55f, 1f);
        }

    }


    public void SaveHelper()
    {
        SaveGlobalHelperData.SaveHelperData(this);
    }

    public void LoadHelper()
    {
        GlobalHelperData data = SaveGlobalHelperData.LoadHelper();

        baseDataSaved = data.baseDataSaved;
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
        else if (baseDataSaved == true)
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

}

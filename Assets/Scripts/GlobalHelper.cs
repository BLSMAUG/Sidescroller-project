using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalHelper : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }
}

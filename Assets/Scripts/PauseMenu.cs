using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isOn=false;

    public void LeaveRoom()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

}

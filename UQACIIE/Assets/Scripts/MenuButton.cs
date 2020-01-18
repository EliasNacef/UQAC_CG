using UnityEngine;
using UnityEngine.SceneManagement;




public class MenuButton : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("TrapGameScene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

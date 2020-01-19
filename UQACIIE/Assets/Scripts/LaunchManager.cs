using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe gestionnaire des changement de scenes
/// </summary>
public class LaunchManager : MonoBehaviour
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

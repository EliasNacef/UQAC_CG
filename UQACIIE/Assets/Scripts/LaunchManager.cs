using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// Classe gestionnaire des changement de scenes
/// </summary>
public class LaunchManager : MonoBehaviour
{
    [SerializeField]
    private InputField toLoad;

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/" + toLoad.text + ".uqac"))
        {
            PlayerPrefs.SetString("Save", toLoad.text);
        }
        else
        {
            PlayerPrefs.SetString("Save", "");
        }
        SceneManager.LoadScene("TrapGameScene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadLevelEditor()
    {
        SceneManager.LoadScene("LevelDesignScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

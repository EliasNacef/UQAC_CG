using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;


/// <summary>
/// Classe gestionnaire des changement de scenes
/// </summary>
public class LaunchManager : MonoBehaviour
{
    [SerializeField]
    private InputField toLoad;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("GameMusic");
    }

    public void LoadMultiGame()
    {
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", toLoad.text);
        SceneManager.LoadScene("MultiGameScene");
    }

    public void LoadSoloGame()
    {
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", toLoad.text);
        SceneManager.LoadScene("SoloGameScene");
    }

    public void LoadLevelGame(string nameLevel)
    {
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", nameLevel);
        SceneManager.LoadScene("SoloGameScene");
    }


    public void LoadMenu()
    {
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/menuMusic");
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadLevelEditor()
    {
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/levelDesignMusic");
        SceneManager.LoadScene("LevelDesignScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MakeHurt()
    {
        Animator animator1 = GameObject.Find("Monster1").GetComponent<Animator>();
        Animator animator2 = GameObject.Find("Monster2").GetComponent<Animator>();
        animator1.SetBool("Title", true);
        animator2.SetBool("Title", true);
        StartCoroutine(DisableEasterEgg());
    }

    private IEnumerator DisableEasterEgg()
    {
        Animator animator1 = GameObject.Find("Monster1").GetComponent<Animator>();
        Animator animator2 = GameObject.Find("Monster2").GetComponent<Animator>();
        yield return new WaitForSeconds(0.05f);
        animator1.SetBool("Title", false);
        animator2.SetBool("Title", false);
    }
}

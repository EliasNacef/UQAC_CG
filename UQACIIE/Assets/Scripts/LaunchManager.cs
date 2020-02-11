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
    private InputField toLoad = null;
    private AudioManager audioManager;
    [SerializeField]
    private Animator transition = null;
    [SerializeField]
    private Animator middleTransition = null;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("GameMusic");
    }

    /// <summary>
    /// Charger le mode multijoueur
    /// </summary>
    public void LoadMultiGame()
    {
        StartCoroutine(Transition("MultiGameScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", toLoad.text);
    }

    /// <summary>
    /// Charger le mode 1 joueur
    /// </summary>
    public void LoadSoloGame()
    {
        StartCoroutine(Transition("SoloGameScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", toLoad.text);
    }

    /// <summary>
    /// Charger un niveau predefini en Solo
    /// </summary>
    /// <param name="nameLevel"></param>
    public void LoadSoloLevelGame(string nameLevel)
    {
        StartCoroutine(Transition("SoloGameScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", nameLevel);
    }


    /// <summary>
    /// Charger un niveau predefini en Multi
    /// </summary>
    /// <param name="nameLevel"></param>
    public void LoadMultiLevelGame(string nameLevel)
    {
        StartCoroutine(Transition("MultiGameScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", nameLevel);
    }
    /// <summary>
    /// Charger le menu principal
    /// </summary>
    public void LoadMenu()
    {
        StartCoroutine(Transition("MenuScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/menuMusic");
    }

    /// <summary>
    /// Charger le mode createur de maps
    /// </summary>
    public void LoadLevelEditor()
    {
        StartCoroutine(Transition("LevelDesignScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/levelDesignMusic");
    }

    /// <summary>
    /// Quitter le jeu
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Easter egg qui blesse les monstres du titre
    /// </summary>
    public void MakeHurt()
    {
        Animator animator1 = GameObject.Find("Monster1").GetComponent<Animator>();
        Animator animator2 = GameObject.Find("Monster2").GetComponent<Animator>();
        animator1.SetTrigger("Title");
        animator2.SetTrigger("Title");
    }

    /// <summary>
    /// Transition de la scene d'introduction
    /// </summary>
    /// <param name="name"></param>
    /// <returns> Temps permettant d'animer correctement la scene </returns>
    private IEnumerator Transition(string name)
    {
        transition.SetTrigger("Start");
        middleTransition.SetTrigger("Start");
        yield return new WaitForSeconds(0.0f);
        SceneManager.LoadScene(name);
    }
}

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

    public void LoadMultiGame()
    {
        StartCoroutine(Transition("MultiGameScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", toLoad.text);
    }

    public void LoadSoloGame()
    {
        StartCoroutine(Transition("SoloGameScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", toLoad.text);
    }

    public void LoadLevelGame(string nameLevel)
    {
        StartCoroutine(Transition("SoloGameScene"));

        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/gameMusic");
        PlayerPrefs.SetString("Save", nameLevel);
    }


    public void LoadMenu()
    {
        StartCoroutine(Transition("MenuScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/menuMusic");
    }

    public void LoadLevelEditor()
    {
        StartCoroutine(Transition("LevelDesignScene"));
        Sound s = Array.Find(audioManager.sounds, item => item.name == "GameMusic");
        s.source.clip = Resources.Load<AudioClip>("Prefab/Sounds/levelDesignMusic");
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

    private IEnumerator Transition(string name)
    {
        transition.SetTrigger("Start");
        middleTransition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }
}

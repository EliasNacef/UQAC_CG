using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaifForIntroAnimation());
    }

    IEnumerator WaifForIntroAnimation()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        yield return new WaitForSeconds(1.5f);
        audioManager.Play("Intro1");
        yield return new WaitForSeconds(0.8f);
        audioManager.Play("Intro2");
        yield return new WaitForSeconds(0.2f);
        audioManager.Play("Intro3");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MenuScene");
    }
}

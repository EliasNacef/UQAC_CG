using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Classe qui gerer la scene d'introduction du jeu
/// </summary>
public class IntroManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitForIntroAnimation());
    }

    /// <summary>
    /// Permet de gerer l'animation du titre en introduction correctement
    /// </summary>
    /// <returns> Temps necessaire </returns>
    IEnumerator WaitForIntroAnimation()
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

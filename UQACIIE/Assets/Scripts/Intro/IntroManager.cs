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
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MenuScene");
    }
}

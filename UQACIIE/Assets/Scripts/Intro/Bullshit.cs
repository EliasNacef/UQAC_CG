﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullshit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(truc());
    }

    IEnumerator truc()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("IntroScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe gestionnaire de l'UI des infos
/// </summary>
public class InfoUI : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager = null; // map
    [SerializeField]
    private Text nbTraps = null; // Score de celui en train de jouer


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        nbTraps.text = "Pièges restants : " + gameManager.nbTraps; // Nombre de pieges restants
    }
}
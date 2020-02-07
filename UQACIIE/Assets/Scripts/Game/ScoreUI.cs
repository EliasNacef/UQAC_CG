using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe gestionnaire de l'UI de score
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private Player player = null; // Player en train de joueur
    [SerializeField]
    private Player spectate = null; // Player en train d'observer
    [SerializeField]
    private Text playerScore = null; // Score de celui en train de jouer
    [SerializeField]
    private Text spectateScore = null; // Score de celui en train d'observer

    void Update()
    {
        playerScore.text = "Player : " + player.Life; // Texte score de joueur
        spectateScore.text = "Spectate : " + spectate.Life; // Texte score du spectateur
    }
}
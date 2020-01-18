using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Player spectate;

    [SerializeField]
    private Text playerScore;
    [SerializeField]
    private Text spectateScore;


    // Update is called once per frame
    void Update()
    {
        playerScore.text = "Player : " + player.Life;
        spectateScore.text = "Spectate : " + spectate.Life;
    }
}

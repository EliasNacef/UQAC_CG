using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege faisant des dommages
/// </summary>
public class KillTrap : Trap
{
    /// <summary>
    /// Activation du piege : va faire des degats au Player player
    /// </summary>
    /// <param name="player"> Joueur ayant declenche le piege </param>
    override public void Activate(Player player)
    {
        gameManager.trapAlreadySet = true; // On a ete touche par un piege donc on ne peut plus en placer un
        isActivated = true; // Le piege a ete active
        animator.SetBool("isActivated", isActivated); // Animation du piege enclenche
        FindObjectOfType<AudioManager>().Play("BoomTrap");
        player.Hurt(); // Infliger des degats au joueur
        StartCoroutine(Desactivate()); // Desactivation du piege
    }

}

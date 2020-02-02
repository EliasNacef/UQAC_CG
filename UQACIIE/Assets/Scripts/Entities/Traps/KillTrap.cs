﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege faisant des dommages
/// </summary>
public class KillTrap : Trap
{
    private void Start()
    {
        name = "Piège";
    }

    /// <summary>
    /// Activation du piege : va faire des degats au Player player
    /// </summary>
    /// <param name="player"> Joueur ayant declenche le piege </param>
    override public void Activate(Entity entity)
    {
        isActivated = true; // Le piege a ete active
        animator.SetBool("isActivated", isActivated); // Animation du piege enclenche
        FindObjectOfType<AudioManager>().Play("BearTrap");
        entity.Hurt(); // Infliger des degats a l'entite
        StartCoroutine(Desactivate()); // Desactivation du piege
    }

}

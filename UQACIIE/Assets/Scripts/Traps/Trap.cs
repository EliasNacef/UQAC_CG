using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere des pieges : les classes filles auront des effets
/// </summary>
public class Trap : MonoBehaviour
{

    protected bool isActivated; // Le piege a-t-il ete declenche
    public Animator animator; // Animator du piege (l'explosion)

    void Start()
    {
        isActivated = false; // Le piege est initialement non declenche
    }

    /// <summary>
    /// Activation du piege sur le Player player
    /// </summary>
    /// <param name="player"> Joueur qui a enclenche le piege </param>
    virtual public void Activate(Player player)
    {
        MapManager.trapAlreadySet = true;
        isActivated = true;
        animator.SetBool("isActivated", isActivated); // Active l'animation du piege
        Debug.Log("Un Trap de base a été enclenché, pas une sous classe..");
    }


}

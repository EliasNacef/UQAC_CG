using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour d'une case
/// </summary>
public class PushTrap : Trap
{

    /// <summary>
    /// Activation du piege : pousse ce qu'il y a autour si possible
    /// </summary>
    /// <param name="player"> Joueur ayant declenche le piege </param>
    override public void Activate(Player player)
    {
        mapManager.trapAlreadySet = true; // On a ete touche par un piege donc on ne peut plus en placer un
        isActivated = true; // Le piege a ete active
        animator.SetBool("isActivated", isActivated); // Animation du piege enclenche
        // TODO : Pour chaque objets a cote du piege, pousser de une case dans sla bonne direction.
        // TODO !
        mapManager.UpdateAroundPosition(this.gameObject);
        Vector3 frontPositionEntity = mapManager.tilemap.GetCellCenterLocal(mapManager.frontCellInt); // La future position du piege
        mapManager.UpdateEntitiesArrays(); // Mise a jour des traps de la liste avant de la parcourir
        foreach (Entity entity in mapManager.entities) // verifie si un piege est deja sur la position de selection
        {
            // Si il y a une entite devant le piege
            if (frontPositionEntity == mapManager.tilemap.WorldToLocal(entity.transform.position))
            {
               // TODO : FAIRE BOUGER LE S ENTITES
            }
        }
        player.Wait(); // Infliger des degats au joueur
        StartCoroutine(Desactivate()); // Desactivation du piege
    }




}

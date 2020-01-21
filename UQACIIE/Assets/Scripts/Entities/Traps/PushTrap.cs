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
        Vector3Int CellEntity = mapManager.grid.GetLocalPosition(mapManager.currentCellInt); // La position du PushTrap
        Vector3Int frontCellEntity = new Vector3Int(0, 1, 0) + CellEntity;
        Vector3Int behindCellEntity = new Vector3Int(0, -1, 0) + CellEntity;
        Vector3Int rightCellEntity = new Vector3Int(1, 0, 0) + CellEntity;
        Vector3Int leftCellEntity = new Vector3Int(-1, 0, 0) + CellEntity; // TODO : Corriger la gridMap pour pas qu'elle interagisse en dehors de sa taille(width et height)
        if (mapManager.grid.GetValue(frontCellEntity.x, frontCellEntity.y) != null)
        {
            // PUSH UP
            //Debug.Log("Local front Entity" + Mathf.FloorToInt(frontCellEntity.x) + Mathf.FloorToInt(frontCellEntity.y));
            if (mapManager.grid.CheckGrid(frontCellEntity.x, frontCellEntity.y + 1))
            {
                mapManager.grid.MoveEntity(frontCellEntity.x, frontCellEntity.y, new Vector3Int(0, 1, 0));
            }
        }
        player.Wait(); // Faire attendre le joueur
        StartCoroutine(Desactivate()); // Desactivation du piege
    }




}

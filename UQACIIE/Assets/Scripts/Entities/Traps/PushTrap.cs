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
        GridMap grid = gameManager.map.grid;
        isActivated = true; // Le piege a ete active
        animator.SetBool("isActivated", isActivated); // Animation du piege enclenche
        FindObjectOfType<AudioManager>().Play("BoomTrap");
        gameManager.map.UpdateAroundPosition(this.gameObject);
        Vector3Int CellEntity = grid.GetLocalPosition(gameManager.map.currentCellInt); // La position du PushTrap
        Vector3Int frontCellEntity = new Vector3Int(0, 1, 0) + CellEntity;
        Vector3Int behindCellEntity = new Vector3Int(0, -1, 0) + CellEntity;
        Vector3Int rightCellEntity = new Vector3Int(1, 0, 0) + CellEntity;
        Vector3Int leftCellEntity = new Vector3Int(-1, 0, 0) + CellEntity; // TODO : Corriger la gridMap pour pas qu'elle interagisse en dehors de sa taille(width et height)
        // PUSH UP
        if (grid.GetValue(frontCellEntity.x, frontCellEntity.y) != null)
        {
            if (grid.CheckGrid(frontCellEntity.x, frontCellEntity.y + 1))
            {
                grid.MoveEntity(frontCellEntity.x, frontCellEntity.y, new Vector3Int(0, 1, 0));
            }
        }
        // PUSH DOWN
        if (grid.GetValue(behindCellEntity.x, behindCellEntity.y) != null)
        {
            if (grid.CheckGrid(behindCellEntity.x, behindCellEntity.y - 1))
            {
                grid.MoveEntity(behindCellEntity.x, behindCellEntity.y, new Vector3Int(0, -1, 0));
            }
        }
        // PUSH RIGHT
        if (grid.GetValue(rightCellEntity.x, rightCellEntity.y) != null)
        {
            if (grid.CheckGrid(rightCellEntity.x + 1, rightCellEntity.y))
            {
                grid.MoveEntity(rightCellEntity.x, rightCellEntity.y, new Vector3Int(1, 0, 0));
            }
        }
        // PUSH LEFT
        if (grid.GetValue(leftCellEntity.x, leftCellEntity.y) != null)
        {
            if (grid.CheckGrid(leftCellEntity.x - 1, leftCellEntity.y))
            {
                grid.MoveEntity(leftCellEntity.x, leftCellEntity.y, new Vector3Int(-1, 0, 0));
            }
        }




        player.Wait(); // Faire attendre le joueur
        StartCoroutine(Desactivate()); // Desactivation du piege
    }




}

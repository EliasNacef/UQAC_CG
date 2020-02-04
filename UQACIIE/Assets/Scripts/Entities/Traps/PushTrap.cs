using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour d'une case
/// </summary>
public class PushTrap : Trap
{

    private void Start()
    {
        name = "Pousseur";
    }

    /// <summary>
    /// Activation du piege : pousse ce qu'il y a autour si possible
    /// </summary>
    /// <param name="player"> Joueur ayant declenche le piege </param>
    override public void Activate(Entity entity)
    {
        GridMap grid = gameManager.map.grid;
        isActivated = true; // Le piege a ete active
        animator.SetBool("isActivated", isActivated); // Animation du piege enclenche
        FindObjectOfType<AudioManager>().Play("WindTrap");
        gameManager.map.UpdateAroundPosition(this.gameObject);
        Vector3Int CellEntity = grid.GetLocalPosition(gameManager.map.currentCellInt); // La position du PushTrap
        Vector3Int frontCellEntity = new Vector3Int(0, 1, 0) + CellEntity;
        Vector3Int behindCellEntity = new Vector3Int(0, -1, 0) + CellEntity;
        Vector3Int rightCellEntity = new Vector3Int(1, 0, 0) + CellEntity;
        Vector3Int leftCellEntity = new Vector3Int(-1, 0, 0) + CellEntity; // TODO : Corriger la gridMap pour pas qu'elle interagisse en dehors de sa taille(width et height)
        PushAround(grid, frontCellEntity, behindCellEntity, rightCellEntity, leftCellEntity);
        gameManager.player.GetComponent<Player>().Wait(); // Faire attendre le joueur
        StartCoroutine(Desactivate()); // Desactivation du piege
    }



    private void PushAround(GridMap grid, Vector3Int frontCellEntity, Vector3Int behindCellEntity, Vector3Int rightCellEntity, Vector3Int leftCellEntity)
    {
        // PUSH UP
        if (grid.GetValue(frontCellEntity.x, frontCellEntity.y) != null)
        {
            if (grid.CheckGrid(frontCellEntity.x, frontCellEntity.y + 1) || grid.GetValue(frontCellEntity.x, frontCellEntity.y + 1) is Trap)
            {
                grid.MoveEntity(frontCellEntity.x, frontCellEntity.y, new Vector3Int(0, 1, 0));
            }
        }
        // PUSH DOWN
        if (grid.GetValue(behindCellEntity.x, behindCellEntity.y) != null)
        {
            if (grid.CheckGrid(behindCellEntity.x, behindCellEntity.y - 1) || grid.GetValue(behindCellEntity.x, behindCellEntity.y - 1) is Trap)
            {
                grid.MoveEntity(behindCellEntity.x, behindCellEntity.y, new Vector3Int(0, -1, 0));
            }
        }
        // PUSH RIGHT
        if (grid.GetValue(rightCellEntity.x, rightCellEntity.y) != null)
        {
            if (grid.CheckGrid(rightCellEntity.x + 1, rightCellEntity.y) || grid.GetValue(rightCellEntity.x + 1, rightCellEntity.y) is Trap)
            {
                grid.MoveEntity(rightCellEntity.x, rightCellEntity.y, new Vector3Int(1, 0, 0));
            }
        }
        // PUSH LEFT
        if (grid.GetValue(leftCellEntity.x, leftCellEntity.y) != null)
        {
            if (grid.CheckGrid(leftCellEntity.x - 1, leftCellEntity.y) || grid.GetValue(leftCellEntity.x - 1, leftCellEntity.y) is Trap)
            {
                grid.MoveEntity(leftCellEntity.x, leftCellEntity.y, new Vector3Int(-1, 0, 0));
            }
        }
    }


}

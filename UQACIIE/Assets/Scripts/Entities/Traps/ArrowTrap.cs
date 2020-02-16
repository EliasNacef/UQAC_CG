using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour d'une case
/// </summary>
public class ArrowTrap : Trap
{
    protected Vector3Int direction = new Vector3Int(0, 0, 0);

    private void Start()
    {
        name = "Flèche";
    }

    /// <summary>
    /// Activation du piege : pousse ce qu'il y a autour si possible
    /// </summary>
    /// <param name="player"> Joueur ayant declenche le piege </param>
    override public void Activate(Entity entity)
    {
        GridMap grid = gameManager.map.grid;
        AudioManager.instance.Play("WindTrap");
        gameManager.map.UpdateAroundPosition(this.gameObject);
        Vector3Int cellEntity = grid.GetLocalPosition(gameManager.map.currentCellInt); // La position du PushTrap
        PushTowardsDirection(grid, entity, cellEntity, direction);
        gameManager.playerGO.GetComponent<Player>().Wait(); // Faire attendre le joueur
        Desactivate(); // Desactivation du piege
    }


    /// <summary>
    /// Pousse l'entite d'une case  dans la bonne direction
    /// </summary>
    /// <param name="grid"> La grille </param>
    /// <param name="entity"> L'entite </param>
    /// <param name="cellEntity"> La cellule de l'entite </param>
    /// <param name="direction"> Translation a effectuer </param>
    private void PushTowardsDirection(GridMap grid, Entity entity, Vector3Int cellEntity, Vector3Int direction)
    {
        Vector3Int directionCellEntity = direction + cellEntity;
        // PUSH
        if (entity is Player && (grid.CheckGrid(directionCellEntity.x, directionCellEntity.y) || !(grid.GetValue(directionCellEntity.x, directionCellEntity.y) is Block)))
        {
            Vector3 playerPosition = entity.transform.position;
            Vector3 futurePosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z) + direction;
            Vector3Int cellPosition = gameManager.map.grid.GetLocalPosition(futurePosition - new Vector3(0.3f, 0.3f, 0f));
            Entity inFrontEntity = gameManager.map.grid.GetValue(cellPosition.x, cellPosition.y);
            if (entity == null || !(entity is Block))
            {
                entity.transform.position = futurePosition; // Déplacement sur la future cellule
                AudioManager.instance.Play("MoveSound");
            }
        }
        else if (grid.GetValue(cellEntity.x, cellEntity.y) != null)
        {
            if (grid.CheckGrid(directionCellEntity.x, directionCellEntity.y) || grid.GetValue(directionCellEntity.x, directionCellEntity.y) is Trap)
            {
                grid.MoveEntity(cellEntity.x, cellEntity.y, direction);
            }
        }
    }


}

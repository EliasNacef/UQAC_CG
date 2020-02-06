using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour d'une case
/// </summary>
public class ZRightTrap : Trap
{
    private Vector3Int direction = new Vector3Int(1, 0, 0);

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
        isActivated = true; // Le piege a ete active
        animator.SetBool("isActivated", isActivated); // Animation du piege enclenche
        FindObjectOfType<AudioManager>().Play("WindTrap");
        gameManager.map.UpdateAroundPosition(this.gameObject);
        Vector3Int cellEntity = grid.GetLocalPosition(gameManager.map.currentCellInt); // La position du PushTrap
        PushTowardsDirection(grid, entity, cellEntity, direction);
        gameManager.player.GetComponent<Player>().Wait(); // Faire attendre le joueur
        StartCoroutine(Desactivate()); // Desactivation du piege
    }



    private void PushTowardsDirection(GridMap grid, Entity entity, Vector3Int cellEntity, Vector3Int direction)
    {
        Vector3Int directionCellEntity = direction + cellEntity;
        // PUSH
        if(entity is Player)
        {
            Debug.Log("Player");
            Vector3 playerPosition = entity.transform.position;
            Vector3 futurePosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z) + direction;
            Vector3Int cellPosition = gameManager.map.grid.GetLocalPosition(futurePosition - new Vector3(0.3f, 0.3f, 0f));
            Entity inFrontEntity = gameManager.map.grid.GetValue(cellPosition.x, cellPosition.y);
            if (entity == null || !(entity is Block))
            {
                entity.transform.position = futurePosition; // Déplacement sur la future cellule
                FindObjectOfType<AudioManager>().Play("MoveSound");
            }
        }
        else if (grid.GetValue(cellEntity.x, cellEntity.y) != null)
        {
            Debug.Log("Block");

            if (grid.CheckGrid(directionCellEntity.x, directionCellEntity.y) || grid.GetValue(directionCellEntity.x, directionCellEntity.y) is Trap)
            {
                grid.MoveEntity(cellEntity.x, cellEntity.y, direction);
            }
        }
    }


}

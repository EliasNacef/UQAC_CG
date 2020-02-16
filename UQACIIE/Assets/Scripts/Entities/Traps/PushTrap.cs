using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour de sa position
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
    /// <param name="player"> Entite ayant declenche le piege </param>
    override public void Activate(Entity entity)
    {
        if (!(entity is KillTrap))
        {
            GridMap grid = gameManager.map.grid;
            AudioManager.instance.Play("WindTrap");
            gameManager.map.UpdateAroundPosition(this.gameObject);
            Vector3Int CellEntity = grid.GetLocalPosition(gameManager.map.currentCellInt); // La position du PushTrap
            Vector3Int frontCellEntity = new Vector3Int(0, 1, 0) + CellEntity;
            Vector3Int behindCellEntity = new Vector3Int(0, -1, 0) + CellEntity;
            Vector3Int rightCellEntity = new Vector3Int(1, 0, 0) + CellEntity;
            Vector3Int leftCellEntity = new Vector3Int(-1, 0, 0) + CellEntity;
            PushAround(grid, frontCellEntity, behindCellEntity, rightCellEntity, leftCellEntity);
            gameManager.playerGO.GetComponent<Player>().Wait(); // Faire attendre le joueur
            Desactivate(); // Desactivation du piege
        }
    }


    /// <summary>
    /// Pousse les objets sur les cellules designee
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="frontCellEntity"> Cellule dont l'objet doit etre pousse devant </param>
    /// <param name="behindCellEntity"> Cellule dont l'objet doit etre pousse derriere </param>
    /// <param name="rightCellEntity"> Cellule dont l'objet doit etre pousse droite </param>
    /// <param name="leftCellEntity"> Cellule dont l'objet doit etre pousse gauche </param>
    private void PushAround(GridMap grid, Vector3Int frontCellEntity, Vector3Int behindCellEntity, Vector3Int rightCellEntity, Vector3Int leftCellEntity)
    {
        bool isMulti = (gameManager is MultiGameManager);
        bool checkNextValue = false;
        bool killToKill = false;
        Entity nextValue = null;
        Entity front = grid.GetValue(frontCellEntity.x, frontCellEntity.y);
        Entity down = grid.GetValue(behindCellEntity.x, behindCellEntity.y);
        Entity left = grid.GetValue(leftCellEntity.x, leftCellEntity.y);
        Entity right = grid.GetValue(rightCellEntity.x, rightCellEntity.y);
        // PUSH UP
        if (front != null && (isMulti || !(front is KillTrap)))
        {
            checkNextValue = grid.CheckGrid(frontCellEntity.x, frontCellEntity.y + 1);
            nextValue = grid.GetValue(frontCellEntity.x, frontCellEntity.y + 1);
            killToKill = front is KillTrap && nextValue is KillTrap;
            if ((checkNextValue || nextValue is Trap) && !killToKill)
            {
                grid.MoveEntity(frontCellEntity.x, frontCellEntity.y, new Vector3Int(0, 1, 0));
            }
        }
        // PUSH DOWN
        if (down != null && (isMulti || !(down is KillTrap)))
        {
            checkNextValue = grid.CheckGrid(behindCellEntity.x, behindCellEntity.y - 1);
            nextValue = grid.GetValue(behindCellEntity.x, behindCellEntity.y - 1);
            killToKill = down is KillTrap && nextValue is KillTrap;
            if ((checkNextValue || nextValue is Trap) && !killToKill)
            {
                grid.MoveEntity(behindCellEntity.x, behindCellEntity.y, new Vector3Int(0, -1, 0));
            }
        }
        // PUSH RIGHT
        if (right != null && (isMulti || !(right is KillTrap)))
        {
            checkNextValue = grid.CheckGrid(rightCellEntity.x + 1, rightCellEntity.y);
            nextValue = grid.GetValue(rightCellEntity.x + 1, rightCellEntity.y);
            killToKill = right is KillTrap && nextValue is KillTrap;
            if ((checkNextValue || nextValue is Trap) && !killToKill)
            {
                grid.MoveEntity(rightCellEntity.x, rightCellEntity.y, new Vector3Int(1, 0, 0));
            }
        }
        // PUSH LEFT
        if (left != null && (isMulti || !(left is KillTrap)))
        {
            checkNextValue = grid.CheckGrid(leftCellEntity.x - 1, leftCellEntity.y);
            nextValue = grid.GetValue(leftCellEntity.x - 1, leftCellEntity.y);
            killToKill = left is KillTrap && nextValue is KillTrap;
            if ((checkNextValue || nextValue is Trap) && !killToKill)
            {
                grid.MoveEntity(leftCellEntity.x, leftCellEntity.y, new Vector3Int(-1, 0, 0));
            }
        }
    }


}

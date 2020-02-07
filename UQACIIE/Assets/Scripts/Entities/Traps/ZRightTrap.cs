using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour d'une case
/// </summary>
public class ZRightTrap : ArrowTrap
{

    private void Start()
    {
        name = "Droite";
        direction = new Vector3Int(1, 0, 0);
        isStatic = true;
    }



}
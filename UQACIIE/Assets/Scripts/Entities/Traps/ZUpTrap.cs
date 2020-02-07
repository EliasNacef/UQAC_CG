using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse ce qu'il y a autour d'une case
/// </summary>
public class ZUpTrap : ArrowTrap
{

    private void Start()
    {
        name = "Haut";
        direction = new Vector3Int(0, 1, 0);
        isStatic = true;
    }



}
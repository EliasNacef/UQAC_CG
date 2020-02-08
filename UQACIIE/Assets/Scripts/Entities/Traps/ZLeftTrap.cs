using UnityEngine;


/// <summary>
/// Sous-classe de Trap : piege qui pousse a gauche
/// </summary>
public class ZLeftTrap : ArrowTrap
{

    private void Start()
    {
        name = "Gauche";
        direction = new Vector3Int(-1, 0, 0);
        isStatic = true;
    }
}
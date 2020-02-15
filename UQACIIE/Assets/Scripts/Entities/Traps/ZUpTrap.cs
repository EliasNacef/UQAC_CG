using UnityEngine;

/// <summary>
/// Sous-classe de Trap : piege qui pousse devant
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
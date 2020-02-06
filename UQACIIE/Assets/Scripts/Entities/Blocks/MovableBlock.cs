using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : Block
{
    private void Start()
    {
        name = "Tonneau";
    }

    
    /// <summary>
    /// Blesser le joueur
    /// </summary>
    override
    public void Hurt()
    {
        gameManager.map.roundTraps.Remove(this);
        Destroy(this.gameObject);
    }
}

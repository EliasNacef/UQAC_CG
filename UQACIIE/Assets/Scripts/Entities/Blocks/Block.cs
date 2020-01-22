using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere des blocks : les classes filles sont mobiles ou non
/// </summary>
public class Block : Entity
{

    void Start()
    {
        mapManager = GameObject.Find("Tiles").GetComponent<MapManager>(); // On load le MapManager
    }

}

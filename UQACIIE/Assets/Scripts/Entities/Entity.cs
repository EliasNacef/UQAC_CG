﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe designant les differentes entites qui seront sur la grid de la map
/// </summary>
public class Entity : MonoBehaviour
{
    protected MapManager mapManager;
    public bool isStatic;

    void Start()
    {
        isStatic = false;
        mapManager = GameObject.Find("Tiles").GetComponent<MapManager>(); // On load le MapManager
    }


    void Update()
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe designant les differentes entites qui seront sur la grid de la map
/// </summary>
public class Entity : MonoBehaviour
{
    protected GameManager gameManager;
    public bool isStatic;

    void Start()
    {
        isStatic = false;
        gameManager = GameObject.Find("Tiles").GetComponent<GameManager>(); // On load le MapManager
    }


    void Update()
    {
        
    }
}

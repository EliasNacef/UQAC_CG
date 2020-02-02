using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe designant les differentes entites qui seront sur la grid de la map
/// </summary>
public class Entity : MonoBehaviour
{
    new public string name;
    protected GameManager gameManager;
    public bool isStatic;

    void Awake()
    {
        isStatic = false;
        gameManager = FindObjectOfType<GameManager>(); // On load le MapManager
    }

    /// <summary>
    /// Blesser le joueur
    /// </summary>
    virtual public void Hurt()
    {
        Debug.Log("Nothing happens..");
    }
}

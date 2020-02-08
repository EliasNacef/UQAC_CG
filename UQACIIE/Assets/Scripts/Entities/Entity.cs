using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe designant les differentes entites qui seront sur la grid de la map
/// </summary>
public class Entity : MonoBehaviour
{
    new public string name; // Nom de l'entite
    protected GameManager gameManager; // Gestionnaire de jeu
    public bool isStatic; // L'entite est-elle statique ?
    public bool isActive = true; // L'entite est-elle active ?


    void Awake()
    {
        isStatic = false;
        gameManager = FindObjectOfType<GameManager>(); // On load le MapManager
    }

    /// <summary>
    /// Action "Blesser" l'entite (generalement : la detruire)
    /// </summary>
    virtual public void Hurt()
    {
        gameManager.map.trapsToHide.Remove(this);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Inverse l'activite de l'entite
    /// </summary>
    public void SwitchActivity()
    {
        isActive = !isActive;
    }
}

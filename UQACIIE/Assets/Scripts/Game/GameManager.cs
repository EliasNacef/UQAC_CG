using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere gestionnaire du jeu
/// </summary>
public class GameManager : MonoBehaviour
{
    public Map map; // La map
    [SerializeField]
    public GameObject playerGO; // Le gameobject du joueur
    public Entity newEntity; // La nouvelle entite qui sera pose par le joueur

    [SerializeField]
    protected GameObject victoryPanel; // Le panel a afficher en cas de victoire
    [SerializeField]
    protected GameObject failurePanel; // Le panel a afficher en cas de defaite
    [SerializeField]
    protected GameObject pausePanel; // Le panel a afficher en pause

    protected int roundNumberTraps = 1; // Nombre de pieges que l'on peut poser pendant un round
    public int nbTraps; // Nombre de pieges restants pendant le round
    protected int tamponSetTrap; // Tampon pour le int nbTraps
    protected bool endGame = false; // Est-ce la fin du jeu ?



    /// <summary>
    /// Met a jour les positions en fonction du statut joueur/spectateur
    /// </summary>
    virtual public void UpdatePlayersPositions()
    {
        // Mise a jour de la position du joueur au spawn
        playerGO.transform.position = map.spawnPosition;
    }

    /// <summary>
    /// Positionne la camera au dessus du joueur
    /// </summary>
    protected void CameraUpToPlayer()
    {
        Camera.main.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, -8f);
    }

    /// <summary>
    /// Permet de faire suivre le joueur par la camera
    /// </summary>
    protected void CameraFollowPlayer()
    {
        Camera.main.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, Camera.main.transform.position.z);
    }


    /// <summary>
    /// Permet de faire zoomer avec la camera
    /// </summary>
    public void CameraUp()
    {
        if (Camera.main.transform.position.z < -1)
        {
            Camera.main.transform.position += new Vector3(0, 0, 1);
        }
    }

    /// <summary>
    /// Permet de faire dezoomer avec la camera
    /// </summary>
    public void CameraDown()
    {
        if (Camera.main.transform.position.z > -(Mathf.Abs(map.grid.GetWidth()) + Mathf.Abs(map.grid.GetHeight())))
        {
            Camera.main.transform.position += new Vector3(0, 0, -1);
        }
    }
}

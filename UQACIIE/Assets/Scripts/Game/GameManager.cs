using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Map map;
    [SerializeField]
    public GameObject player; // Le joueur
    public Entity newEntity; // La nouvelle entite a placer

    [SerializeField]
    protected GameObject victoryPanel; // Le panel a afficher apres la fin du jeu
    [SerializeField]
    protected GameObject failurePanel; // Le panel a afficher apres la fin du jeu
    [SerializeField]
    protected GameObject pausePanel; // Le panel a afficher en pause

    protected int roundNumberTraps = 1;
    public int nbTraps; // Un piege a-t-il deja ete place pendant le round ?
    protected int tamponSetTrap; // Tampon pour le int nbTraps



    /// <summary>
    /// Met a jour les positions en fonction du statut joueur/spectateur
    /// </summary>
    virtual public void UpdatePlayersPositions()
    {
        // Mise a jour de la position du joueur au spawn
        player.transform.position = map.spawnPosition;
    }


    protected void CameraUpToPlayer()
    {
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -6f);
    }

    protected void CameraFollowPlayer()
    {
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.IO;


/// <summary>
/// Classe de gestion du mode un joueur
/// </summary>
public class SoloGameManager : GameManager
{
    private PlayerMovement playerMovement;
    private Player player;

    private void Start()
    {
        map.LoadLevel(); // On charge la map
        PutSpawn();
        playerMovement = playerGO.GetComponent<PlayerMovement>();
        player = playerGO.GetComponent<Player>();
        // Parametres initiaux
        nbTraps = map.idealNumberOfTraps; // Nb de bombes initial
        UpdateAbilities(); // On update les abilites
        UpdatePlayersPositions();
        map.UpdateAroundPosition(playerGO); // Cellule du joueur et celle devant lui mises a jour
        CameraUpToPlayer(); // On positionne la camera au dessus du joueur
    }



    void Update()
    {
        if (!endGame)
        {
            TestEndGame(); // Testons si c'est la fin du jeu
            CameraFollowPlayer();
            // La case de selection se deplace avec le joueur.
            map.SetSelectionTile(playerGO);
            if (Input.GetButtonDown("Jump")) // Si on appuie sur 'Espace'
                TryToSetTrap(); // Essayer de poser un piege
            else if (Input.GetButtonDown("R"))
            {
                if (playerMovement.canMove) map.RotateSelection();
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                Pause();
            }
        }
        else
        {
            playerMovement.canMove = false;
            map.SetSelectionTile(playerGO);
            return;
        }
    }


    /// <summary>
    /// Tente de placer un piege sur la position de selection du piege.
    /// </summary>
    void TryToSetTrap()
    {
        if (nbTraps > 0 && !endGame) // Si je peux poser un piege et que ce n'est pas la fin de la manche
        {
            if (map.SetTrap(newEntity)) nbTraps--;
            else Debug.Log("Je ne peux pas poser de piège");
        }
        else // Je ne peux pas poser de piege pour le moment
        {
            Debug.Log("Je ne peux pas poser de piège");
        }
    }

   

    /// <summary>
    /// Met a jour les abilites des joueurs en fonction de leur statut
    /// </summary>
    void UpdateAbilities()
    {
        playerMovement.canMove = true;
    }



    /// <summary>
    /// Teste si c'est la fin du jeu. Si l'un des joueurs n'a plus de vie, on met fin au jeu.
    /// </summary>
    private void TestEndGame()
    {
        if (player.Life <= 0)
        {
            endGame = true;
            // On bloque les mouvement et la possibilite de mettre des pieges
            playerMovement.canMove = false;
            nbTraps = 0;
            FindObjectOfType<AudioManager>().Play("GameOver");
            failurePanel.SetActive(true);
        }
        else if (map.grid.GetLocalPosition(playerGO.transform.position).y >= map.grid.GetLocalPosition(map.endTilemap).y)
        {
            endGame = true;
            // On bloque les mouvement et la possibilite de mettre des pieges
            playerMovement.canMove = false;
            nbTraps = 0;
            FindObjectOfType<AudioManager>().Play("Victory");
            victoryPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Met en pause ou fait reprendre le jeu
    /// </summary>
    private void Pause()
    {
        playerMovement.canMove = pausePanel.activeSelf;
        if (!pausePanel.activeSelf) tamponSetTrap = nbTraps;
        nbTraps = 0;
        if (pausePanel.activeSelf) nbTraps = tamponSetTrap;
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.IO;


/// <summary>
/// Classe gestionnaire de la map. Interagit avec la map en focntion de ce qu'il s'y passe.
/// </summary>
public class SoloGameManager : GameManager
{
    protected bool endGame; // Est-ce la fin du jeu ?

    private void Start()
    {
        map.LoadLevel();
        map.spawnPosition = new Vector3(map.startTilemap.x + Mathf.FloorToInt((map.grid.GetWidth() / 2)) + 0.5f, map.startTilemap.y + 1f, 0f);

        // Parametres initiaux
        nbTraps = map.idealNumberOfTraps; // Nb de bombes initial
        endGame = false; // Ce n'est pas la fin du tour, il vient de commencer
        UpdateAbilities(); // On update les abilites
        UpdatePlayersPositions();
        map.UpdateAroundPosition(player); // Cellule du joueur et celle devant lui mises a jour
    }



    void Update()
    {
        TestEndGame(); // Testons si c'est la fin du jeu

        // La case de selection se deplace avec le joueur.
        map.SetSelectionTile(player);
        if (Input.GetButtonDown("Jump")) // Si on appuie sur 'Espace'
            TryToSetTrap(); // Essayer de poser un piege
        else if(Input.GetButtonDown("R"))
        {
            if (player.GetComponent<PlayerMovement>().canMove) map.RotateSelection();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.transform.position += new Vector3(0, 0, 1);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.transform.position += new Vector3(0, 0, -1);
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
    /// Met a jour les abilites du joueur et du spectateur : le joueur peut bouger mais pas le spectateur
    /// </summary>
    void UpdateAbilities()
    {
        player.GetComponent<PlayerMovement>().canMove = true;
    }





    /// <summary>
    /// Teste si c'est la fin du jeu. Si l'un des joueurs n'a plus de vie, on met fin au jeu.
    /// </summary>
    private void TestEndGame()
    {
        Player p = player.GetComponent<Player>();
        if (p.Life <= 0)
        {
            // On bloque les mouvement et la possibilite de mettre des pieges
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.canMove = false;
            nbTraps = 0;
            failurePanel.SetActive(true);
        }
        else if (map.grid.GetLocalPosition(player.transform.position).y >= map.grid.GetLocalPosition(map.endTilemap).y)
        {
            // On bloque les mouvement et la possibilite de mettre des pieges
            endGame = true;
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.canMove = false;
            UpdatePlayersPositions();
            nbTraps = 0;
            victoryPanel.SetActive(true);
        }
    }

    private void Pause()
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        pm.canMove = pausePanel.activeSelf;
        if (!pausePanel.activeSelf) tamponSetTrap = nbTraps;
        nbTraps = 0;
        if (pausePanel.activeSelf) nbTraps = tamponSetTrap;

        pausePanel.SetActive(!pausePanel.activeSelf);
    }

}

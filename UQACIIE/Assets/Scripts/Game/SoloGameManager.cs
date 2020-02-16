using UnityEngine;

/// <summary>
/// Classe de gestion du mode solo
/// </summary>
public class SoloGameManager : GameManager
{
    private AudioManager audioManager; // Le gestionnaire de sons
    private PlayerMovement playerMovement; // Mouvement du joueur
    private Player player; // Joueur

    private void Start()
    {
        map.LoadLevel(); // On charge la map
        PutSpawn(); // On place le spawn
        playerMovement = playerGO.GetComponent<PlayerMovement>();
        player = playerGO.GetComponent<Player>();

        // Parametres initiaux
        nbTraps = map.idealNumberOfTraps; // Nb de bombes initial
        UpdateAbilities(); // On update les abilites
        UpdatePlayersPositions(); // ON initialise la position du joueur
        map.UpdateAroundPosition(playerGO); // mise a jour de la position des cellules autour du joueur
        CameraUpToPlayer(); // On positionne la camera au dessus du joueur
        audioManager = AudioManager.instance;
    }



    void Update()
    {
        if (!endGame)
        {
            TestEndGame(); // Testons si c'est la fin du jeu
            CameraFollowPlayer(); // Camera suit le joueur
            map.SetSelectionTile(playerGO); // La case de selection se deplace avec le joueur.
            if (Input.GetButtonDown("Jump")) // Si on appuie sur 'Espace'
                TryToSetTrap(); // Essayer de poser un piege
            else if (Input.GetButtonDown("R"))
            {
                if (playerMovement.canMove) map.RotateSelection(); // Faire tourner la tile de selection
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
    /// Met a jour les abilites des joueurs en fonction de leur statut (le joueur peut toujours bouger)
    /// </summary>
    void UpdateAbilities()
    {
        playerMovement.canMove = true;
    }



    /// <summary>
    /// Teste si c'est la fin du jeu. Si le joueur n'a plus de vie, on met fin au jeu.
    /// </summary>
    private void TestEndGame()
    {
        if (player.Life <= 0)
        {
            endGame = true;
            // On bloque les mouvement et la possibilite de mettre des pieges
            playerMovement.canMove = false;
            nbTraps = 0;
            audioManager.Play("GameOver");
            failurePanel.SetActive(true);
        }
        else if (map.grid.GetLocalPosition(playerGO.transform.position).y >= map.grid.GetLocalPosition(map.endTilemap).y)
        {
            endGame = true;
            // On bloque les mouvement et la possibilite de mettre des pieges
            playerMovement.canMove = false;
            nbTraps = 0;
            audioManager.Play("Victory");
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

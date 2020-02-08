using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.IO;


/// <summary>
/// Classe du gamemode Multijoueur : permet de joueur a 2
/// </summary>
public class MultiGameManager : GameManager
{
    private GameObject spectatorGO; // Le gameobject du spectateur (qui attend de jouer)
    [SerializeField]
    private GameObject player1 = null; // GameObject du premier joueur
    [SerializeField]
    private GameObject player2 = null; // GameObject du second joueur

    private int whoPlay; // Entier indiquant le numero de celui qui joue
    private bool endRound; // Est-ce la fin d'un round ?



    private void Start()
    {
        map.LoadLevel(); // On charge la map de jeu
        map.spawnPosition = new Vector3(map.startTilemap.x + Mathf.FloorToInt((map.grid.GetWidth() / 2)) + 0.5f, map.startTilemap.y + 1f, 0f); // Position initiale du joueur
        map.spectatePosition = new Vector3(map.startTilemap.x - 1.5f, map.startTilemap.y + Mathf.FloorToInt((map.grid.GetHeight() / 2)) + 1f, 0f); // Position du spectateur
        GameObject.Find("Tilemap_Base").GetComponent<Tilemap>().SetTile(new Vector3Int(Mathf.FloorToInt(map.spectatePosition.x), Mathf.FloorToInt(map.spectatePosition.y) - 1, 0), Resources.Load<TileBase>("Prefab/Rouge")); // Plateforme du spectateur

        // Parametres initiaux
        whoPlay = 1; // Le joueur 1 commence a jouer
        playerGO = player1; // Le premier a jouer est donc le joueur 1
        spectatorGO = player2; // ..et le spectator est donc le joueur 2
        nbTraps = roundNumberTraps; // Le nombre de pieges pendant le round est le nombre ideal de pieges a poser pendant un round
        endRound = false; // Ce n'est pas la fin du tour, il vient de commencer
        UpdatePlayersPositions(); // On update les positions
        UpdateAbilities(); // On update les abilites des joueurs en fonction de leur statut
        map.UpdateAroundPosition(playerGO); // Cellule du joueur et celle devant lui mises a jour
        CameraUpToPlayer(); // La camera est initialement au dessus du joueur
    }



    void Update()
    {
        if (!endGame) // Tant que le jeu continue
        {
            TestEndGame(); // Testons si c'est la fin du jeu
            CameraFollowPlayer();
            if (endRound) // Est-ce la fin d'un round ?
                StartCoroutine(EndRound()); // Si oui, on y met fin
            else // Sinon
            {
                // La case de selection se deplace avec le joueur.
                map.SetSelectionTile(playerGO);
                if (map.grid.GetLocalPosition(playerGO.transform.position).y >= map.grid.GetLocalPosition(map.endTilemap).y)
                    endRound = true; // Fin du round
                else if (Input.GetButtonDown("Jump")) // Si on appuie sur 'Espace'
                    TryToSetTrap(); // Essayer de poser un piege
                else if (Input.GetButtonDown("R"))
                {
                    if (playerGO.GetComponent<PlayerMovement>().canMove) map.RotateSelection();
                }
                else if (Input.GetButtonDown("Cancel"))
                {
                    Pause();
                }
            }
        }
        else
        {
            playerGO.GetComponent<PlayerMovement>().canMove = false;
            spectatorGO.GetComponent<PlayerMovement>().canMove = false;
            map.SetSelectionTile(playerGO);
            return;
        }
    }



    /// <summary>
    /// Tente de placer un piege sur la position de selection du piege.
    /// </summary>
    void TryToSetTrap()
    {
        if (nbTraps > 0 && !endRound) // Si je peux poser un piege et que ce n'est pas la fin de la manche
        {
            if (map.SetTrap(newEntity))
            {
                nbTraps--; // On a pose un piege
                UpdatePlayersPositions();
            }
            else Debug.Log("Je ne peux pas poser de piège");
        }
        else // Je ne peux pas poser de piege pour le moment
        {
            Debug.Log("Je ne peux pas poser de piège");
        }
    }

    /// <summary>
    /// Changer les joueurs de positions, leurs capacites (mouvement..) et inverse leur statut de joueur/spectateur
    /// </summary>
    void SwitchPlayer()
    {
        if (whoPlay == 1) // Si le joueur 1 joue
        {
            playerGO = player2;
            spectatorGO = player1;
            whoPlay = 2;
        }
        else if(whoPlay == 2) // Sinon si le joueur 2 joue
        {
            playerGO = player1;
            spectatorGO = player2;
            whoPlay = 1;
        }
        else // Erreur
            Debug.Log("whoPlay a une valeur différente de 1 ou 2 !");

        // Update
        UpdatePlayersPositions();
        UpdateAbilities();
    }


    /// <summary>
    /// Met a jour les positions des joueurs en fonction de leur statut
    /// </summary>
    override
    public void UpdatePlayersPositions()
    {
        // Mise a jour des positions
        playerGO.transform.position = map.spawnPosition;
        spectatorGO.transform.position = map.spectatePosition;

        // Le spectateur doit observer le jeu est donc faire face a la game area
        PlayerMovement controllerSpectate = spectatorGO.GetComponent<PlayerMovement>();
        if (!controllerSpectate.m_FacingRight)
            controllerSpectate.Flip();
    }



    /// <summary>
    /// Met a jour les abilites des joueurs en fonction de leur statut
    /// </summary>
    void UpdateAbilities()
    {
        playerGO.GetComponent<PlayerMovement>().canMove = true;
        spectatorGO.GetComponent<PlayerMovement>().canMove = false;
    }



    /// <summary>
    /// Fin du round, reinitialisation des variables du tour et on echange le joueur avec le spectateur
    /// </summary>
    public IEnumerator EndRound()
    {
        map.selectionRotation = new Vector3Int(0, 1, 0);
        nbTraps = roundNumberTraps; // On redonne le nb de pieges initial
        endRound = false;
        foreach(Trap trap in map.trapsToHide)
        {
            if (trap != null) StartCoroutine(trap.Hiding());
        }
        SwitchPlayer(); // Echange de joueurs
        playerGO.GetComponent<PlayerMovement>().canMove = false;
        yield return new WaitForSeconds(0.10f); //Permet d'eviter que le nouveau joueur n'avance a cause de l'input du joueur precedent
        UpdateAbilities();
        CameraUpToPlayer();
        map.trapsToHide.Clear();
    }


    /// <summary>
    /// Teste si c'est la fin du jeu. Si l'un des joueurs n'a plus de vie, on met fin au jeu.
    /// </summary>
    private void TestEndGame()
    {
        Player p = playerGO.GetComponent<Player>();
        Player s = spectatorGO.GetComponent<Player>();
        if (p.Life <= 0 || s.Life <= 0)
        {
            endGame = true;
            // On bloque les mouvement et la possibilite de mettre des pieges
            PlayerMovement pm = playerGO.GetComponent<PlayerMovement>();
            PlayerMovement sm = spectatorGO.GetComponent<PlayerMovement>();
            pm.canMove = false;
            sm.canMove = false;
            nbTraps = 0;
            FindObjectOfType<AudioManager>().Play("GameOver");
            victoryPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Met en pause ou fait reprendre le jeu
    /// </summary>
    private void Pause()
    {
        PlayerMovement pm = playerGO.GetComponent<PlayerMovement>();
        PlayerMovement sm = spectatorGO.GetComponent<PlayerMovement>();
        pm.canMove = pausePanel.activeSelf;
        sm.canMove = false;
        if (!pausePanel.activeSelf) tamponSetTrap = nbTraps;
        nbTraps = 0;
        if (pausePanel.activeSelf) nbTraps = tamponSetTrap;

        pausePanel.SetActive(!pausePanel.activeSelf);
    }
}

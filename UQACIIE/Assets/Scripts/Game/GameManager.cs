using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.IO;


/// <summary>
/// Classe gestionnaire de la map. Interagit avec la map en focntion de ce qu'il s'y passe.
/// </summary>
public class GameManager : MonoBehaviour
{
    public Map map;
    private GameObject player; // Le joueur
    private GameObject spectator; // Le spectateur (qui attend de jouer)
    [SerializeField]
    private GameObject player1; // GameObject du premier joueur
    [SerializeField]
    private GameObject player2; // GameObject du second joueur

    private int whoPlay; // Entier indiquant le numero de celui qui joue
    public Entity newEntity; // La nouvelle entite a placer

    [SerializeField]
    private GameObject victoryPanel; // Le panel a afficher apres la fin du jeu
    [SerializeField]
    private GameObject pausePanel; // Le panel a afficher en pause

    private bool endRound; // Est-ce la fin d'un round ?
    public bool trapAlreadySet; // Un piege a-t-il deja ete place pendant le round ?



    private void Start()
    {
        map.LoadLevel();
        map.spawnPosition = new Vector3(map.startTilemap.x + Mathf.FloorToInt((map.grid.GetWidth() / 2)) + 0.5f, map.startTilemap.y + 1f, 0f);
        map.spectatePosition = new Vector3(map.startTilemap.x - 1.5f, map.startTilemap.y + Mathf.FloorToInt((map.grid.GetHeight() / 2)) + 1f, 0f);
        GameObject.Find("Tilemap_Base").GetComponent<Tilemap>().SetTile(new Vector3Int(Mathf.FloorToInt(map.spectatePosition.x), Mathf.FloorToInt(map.spectatePosition.y) - 1, 0), Resources.Load<TileBase>("Prefab/redTile"));

        // Parametres initiaux
        whoPlay = 1; // Le joueur 1 commence a jouer
        player = player1; // Le player est donc le joueur 1
        spectator = player2; // ..et le spectator est donc le joueur 2
        trapAlreadySet = false; // la bombe n'a pas encore ete placee
        endRound = false; // Ce n'est pas la fin du tour, il vient de commencer
        UpdatePlayersPositions(); // On update les positions
        UpdateAbilities(); // On update les abilites
        map.UpdateAroundPosition(player); // Cellule du joueur et celle devant lui mises a jour
    }



    void Update()
    {
        //camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -12); // La camera suit le joueur
        //Vector3 center = grid.GetLocalPosition(player.transform.position - new Vector3(0.3f, 0.3f, 0f)); // Player trop grand
        //Debug.DrawLine(new Vector3(center.x - 0.5f, center.y - 0.5f, 0), new Vector3(center.x + 0.5f, center.y + 0.5f, 0), Color.blue, 100f);
        //grid.EntitiesDisp(); // TODO Debug pour connaitre les position des entites sur la gridArray de grid
        TestEndGame(); // Testons si c'est la fin du jeu

        if (endRound) // Est-ce la fin d'un round ?
            StartCoroutine(EndRound()); // Si oui, on y met fin
        else // Sinon
        {
            // La case de selection se deplace avec le joueur.
            map.SetSelectionTile(player);
            if (map.grid.GetLocalPosition(player.transform.position).y >= map.grid.GetLocalPosition(map.endTilemap).y)
                endRound = true; // Fin du round
            else if (Input.GetButtonDown("Jump")) // Si on appuie sur 'Espace'
                TryToSetTrap(); // Essayer de poser un piege
            else if(Input.GetButtonDown("R"))
            {
                map.RotateSelection();

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
    }


    /// <summary>
    /// Tente de placer un piege sur la position de selection du piege.
    /// </summary>
    void TryToSetTrap()
    {
        if (!trapAlreadySet && !endRound) // Si je peux poser un piege et que ce n'est pas la fin de la manche
        {
            map.SetTrap(newEntity);
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
            player = player2;
            spectator = player1;
            whoPlay = 2;
        }
        else if(whoPlay == 2) // Sinon si le joueur 2 joue
        {
            player = player1;
            spectator = player2;
            whoPlay = 1;
        }
        else // Erreur
            Debug.Log("whoPlay a une valeur différente de 1 ou 2 !");

        // Update
        UpdatePlayersPositions();
        UpdateAbilities();
    }


    /// <summary>
    /// Met a jour les positions en fonction du statut joueur/spectateur
    /// </summary>
    public void UpdatePlayersPositions()
    {
        // Mise a jour des positions
        player.transform.position = map.spawnPosition;
        spectator.transform.position = map.spectatePosition;

        // Le spectateur doit observer le jeu est donc faire face a la game area
        PlayerMovement controllerSpectate = spectator.GetComponent<PlayerMovement>();
        if (!controllerSpectate.m_FacingRight)
            controllerSpectate.Flip();
    }



    /// <summary>
    /// Met a jour les abilites du joueur et du spectateur : le joueur peut bouger mais pas le spectateur
    /// </summary>
    void UpdateAbilities()
    {
        player.GetComponent<PlayerMovement>().canMove = true;
        spectator.GetComponent<PlayerMovement>().canMove = false;
    }



    /// <summary>
    /// Fin du round, reinitialisation des variables du tour et on echange le joueur avec le spectateur
    /// </summary>
    public IEnumerator EndRound()
    {
        map.selectionRotation = new Vector3Int(0, 1, 0);
        trapAlreadySet = false;
        endRound = false;
        SwitchPlayer(); // Echange de joueurs
        player.GetComponent<PlayerMovement>().canMove = false;
        yield return new WaitForSeconds(0.10f); //Eviter que le joueur n'avance automatiquement avec l'input du joueur precedent
        UpdateAbilities();
    }


    /// <summary>
    /// Teste si c'est la fin du jeu. Si l'un des joueurs n'a plus de vie, on met fin au jeu.
    /// </summary>
    private void TestEndGame()
    {
        Player p = player.GetComponent<Player>();
        Player s = spectator.GetComponent<Player>();
        if (p.Life <= 0 || s.Life <= 0)
        {
            // On bloque les mouvement et la possibilite de mettre des pieges
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            PlayerMovement sm = spectator.GetComponent<PlayerMovement>();
            pm.canMove = false;
            sm.canMove = false;
            trapAlreadySet = true;
            victoryPanel.SetActive(true);
        }
    }

    private void Pause()
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        PlayerMovement sm = spectator.GetComponent<PlayerMovement>();
        pm.canMove = false;
        sm.canMove = false;
        trapAlreadySet = true;
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

}

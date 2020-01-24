using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Classe gestionnaire de la map. Interagit avec la map en focntion de ce qu'il s'y passe.
/// </summary>
public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    public Tilemap tilemap; // Tilemap de de la Game Area
    public Vector3Int startTilemap; // Position de debut de dessin (en bas a gauche)
    public Vector3Int endTilemap; // Position de fin de dessin (en haut a droite)
    public Tile drawingTile; // Tile pour dessiner


    [SerializeField]
    private Tilemap selectionMap; // Tilemap ou la case selectionnee est dessinee
    public TileBase selectionTile; // La TileBase que l'on va dessiner pour la selection
    private Vector3Int selectionRotation;
    public TileBase oldTile; // La TileBase que l'on va stocker avant de redessiner
    public Vector3Int oldPosition;
    public GridMap grid; // La grille de jeu associee a la tilemap de la game area

    [SerializeField]
    public Entity[] entities;
    public Entity newEntity; // La nouvelle entite a instancier
    [SerializeField]
    public Trap[] traps; // La liste des pieges instancies
    public Block[] blocks; // La liste des blocks instancies
    public Player[] players; // La liste des blocks instancies



    private GameObject player; // Le joueur
    private GameObject spectator; // Le spectateur (qui attend de jouer)
    [SerializeField]
    private GameObject player1; // GameObject du premier joueur
    [SerializeField]
    private GameObject player2; // GameObject du second joueur

    private int whoPlay; // Entier indiquant le numero de celui qui joue

    private Vector3 spawnPosition; // La position ou le joueur doit apparaitre
    private Vector3 spectatePosition; // La position ou le spactateur doit observer

    private Vector3 currentCell; // Cellule repere
    private Vector3 frontCell; // Cellule juste devant (au dessus) la currentCell

    public Vector3Int currentCellInt; // Cellule currentCell convertie en vectuer d'entiers avec Floor()
    public Vector3Int frontCellInt; // Cellule frontCell convertie en vectuer d'entiers avec Floor()


    [SerializeField]
    private GameObject victoryPanel; // Le panel a afficher apres la fin du jeu

    private bool endRound; // Est-ce la fin d'un round ?
    public bool trapAlreadySet; // Un piege a-t-il deja ete place pendant le round ?



    private void Start()
    {
        tilemap.ClearAllTiles();
        // TODO : Remplacer ce set Tile par le chargement d'une tilemap preenregistree dans notre level designer
        for(int i = startTilemap.x; i < endTilemap.x; i++)
        {
            for(int j = startTilemap.y; j < endTilemap.y; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), drawingTile);
            }
        }
        grid = new GridMap(tilemap.size.x, tilemap.size.y, 1f, tilemap.origin); // TODO Adapter automatiquement
        camera.transform.position = new Vector3((endTilemap.x + startTilemap.x)/2, (endTilemap.y + startTilemap.y)/2, -Mathf.Max(grid.GetWidth(), grid.GetHeight())); // La camera suit le joueur

        spawnPosition = new Vector3(startTilemap.x + Mathf.FloorToInt((grid.GetWidth() / 2)) + 0.5f, startTilemap.y + 1f, 0f);

        // Parametres initiaux
        whoPlay = 1; // Le joueur 1 commence a jouer
        player = player1; // Le player est donc le joueur 1
        spectator = player2; // ..et le spectator est donc le joueur 2
        trapAlreadySet = false; // la bombe n'a pas encore ete placee
        endRound = false; // Ce n'est pas la fin du tour, il vient de commencer
        selectionRotation = new Vector3Int(0, 1, 0);
        //spawnPosition = GameObject.Find("SpawnPosition").transform.position; // On load la position de spawn dans son vecteur
        spectatePosition = GameObject.Find("SpectatePosition").transform.position; // On load la position de spectate dans son vecteur
        UpdatePlayersPositions(); // On update les positions
        UpdateAbilities(); // On update les abilites
        
        UpdateEntitiesArrays(); // Update des arrays d'entites
        
        UpdateAroundPosition(player); // Cellule du joueur et celle devant lui mises a jour
        oldTile = selectionMap.GetTile(frontCellInt); // Ancienne Tile initialisee
        oldPosition = frontCellInt;
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
            SetSelectionTile();
            if (grid.GetLocalPosition(player.transform.position).y >= grid.GetLocalPosition(endTilemap).y) // TODO CHANGE : Si on arrive en haut de la grille de jeu && mettre la fin du round dans le deplacement ??
                endRound = true; // Fin du round
            else if (Input.GetButtonDown("Jump")) // Si on appuie sur 'Espace'
                TryToSetTrap(); // Essayer de poser un piege
            else if(Input.GetButtonDown("R"))
            {
                if (selectionRotation.x == -1) selectionRotation = new Vector3Int(0, 1, 0);
                else if (selectionRotation.x == 1) selectionRotation = new Vector3Int(0, -1, 0);
                else if (selectionRotation.y == 1) selectionRotation = new Vector3Int(1, 0, 0);
                else if (selectionRotation.y == -1) selectionRotation = new Vector3Int(-1, 0, 0);

            }
        }
    }

    /// <summary>
    /// Met en place la cellule de selection pour poser un piege : dessine une tile devant le joueur pour voir ou le piege sera place
    /// </summary>
    void SetSelectionTile()
    {
        if (!endRound) // Si le round n'est pas fini
        {
            selectionMap.SetTile(oldPosition, oldTile); // Remet en place l'ancienne Tile
            UpdateAroundPosition(player);
            oldTile = selectionMap.GetTile(currentCellInt + selectionRotation); // Stocke la tile avant de la changer
            oldPosition = currentCellInt + selectionRotation;
            selectionMap.SetTile(currentCellInt + selectionRotation, selectionTile); // Dessine la tile de selection
        }
    }

    /// <summary>
    /// Tente de placer un piege sur la position de selection du piege.
    /// </summary>
    void TryToSetTrap()
    {
        bool canSetTrap = true; // Le joueur peut initialement placer un piege si il le souhaite
        Vector3 positionTrap = tilemap.GetCellCenterLocal(currentCellInt + selectionRotation); // La future position du piege

        if (canSetTrap && !trapAlreadySet && !endRound) // Si je peux poser un piege et que ce n'est pas la fin de la manche
        {
            Vector3Int cellTrap = grid.GetLocalPosition(positionTrap);
            if (grid.CheckGrid(cellTrap.x, cellTrap.y))
            {
                var entityInstance = Instantiate(newEntity, positionTrap, Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
                //Debug.Log(cellTrap.ToString());
                grid.SetValue(cellTrap.x, cellTrap.y, entityInstance);
                selectionRotation = new Vector3Int(0, 1, 0);
                trapAlreadySet = true; // Le piege a ete place pendant le round
                UpdatePlayersPositions(); // On reset les positions
            }
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
        player.transform.position = spawnPosition;
        spectator.transform.position = spectatePosition;

        // Le spectateur doit observer le jeu est donc faire face a la game area
        PlayerMovement controllerSpectate = spectator.GetComponent<PlayerMovement>();
        if (!controllerSpectate.m_FacingRight)
            controllerSpectate.Flip();
    }

    /// <summary>
    /// Met a jour les cells autour de go pour travailler avec leurs positions
    /// </summary>
    /// <param name="go"> Gameobject d'observation </param>
    public void UpdateAroundPosition(GameObject go)
    {
        currentCell = tilemap.layoutGrid.WorldToCell(go.transform.position - new Vector3(0f, 0.30f, 0f)); // Cellule ou est le go (le transform du player au milieu de deux cases donc baisse la position en Y)
        frontCell = currentCell + new Vector3(0f, 1f, 0f); // Cellule devant  le go

        currentCellInt = new Vector3Int(Mathf.FloorToInt(currentCell.x), Mathf.FloorToInt(currentCell.y), Mathf.FloorToInt(currentCell.z));
        frontCellInt = new Vector3Int(Mathf.FloorToInt(frontCell.x), Mathf.FloorToInt(frontCell.y), Mathf.FloorToInt(frontCell.z));

    }



    /// <summary>
    /// Met a jour les abilites du joueur et du spectateur : le joueur peut bouger mais pas le spectateur
    /// </summary>
    void UpdateAbilities()
    {
        player.GetComponent<PlayerMovement>().canMove = true;
        spectator.GetComponent<PlayerMovement>().canMove = false;
    }


    public void UpdateEntitiesArrays()
    {
        // Liste des pièges
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        // Liste des blocks
        blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        // Liste des players
        players = GameObject.Find("Players").GetComponentsInChildren<Player>();
        // Liste des entites puis remplissage des entites presentes dans cette liste
        entities = new Entity[traps.Length + blocks.Length + players.Length];
        traps.CopyTo(entities, 0);
        blocks.CopyTo(entities, traps.Length);
        players.CopyTo(entities, traps.Length + blocks.Length);
    }


    /// <summary>
    /// Fin du round, reinitialisation des variables du tour et on echange le joueur avec le spectateur
    /// </summary>
    public IEnumerator EndRound()
    {
        selectionRotation = new Vector3Int(0, 1, 0);
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
            PlayerMovement sm = player.GetComponent<PlayerMovement>();
            pm.canMove = false;
            sm.canMove = false;
            trapAlreadySet = true;
            victoryPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Sauvegarde le level en cours
    /// </summary>
    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this); // On a sauvegarde le level
    }


    /// <summary>
    /// Load le level sauvegarde
    /// </summary>
    public void LoadLevel() // TODO REMPLIR LE GRID
    {
        LevelData data = SaveSystem.LoadLevel();

        UpdateEntitiesArrays();
        foreach (Trap trap in traps) //  on vide le tableau de traps
        {
            Object.Destroy(trap.gameObject);
        }

        for (int i = 0; i < data.trapsPositions.GetLength(0); i++)
        {
            Vector3 v = new Vector3(data.trapsPositions[i,0], data.trapsPositions[i, 1], data.trapsPositions[i, 2]);
            Instantiate(newEntity, v, Quaternion.identity, GameObject.Find("Traps").transform);
        }
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
    }
}

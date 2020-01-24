using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


/// <summary>
/// Classe gestionnaire de la map. Interagit avec la map en focntion de ce qu'il s'y passe.
/// </summary>
public class LevelDesignManager : MonoBehaviour
{

    [SerializeField]
    public Tilemap tilemap; // Tilemap de de la Game Area
    public Vector3Int startTilemap; // Position de debut de dessin (en bas a gauche)
    public Vector3Int endTilemap; // Position de fin de dessin (en haut a droite)
    public Tile drawingTile; // Tile pour dessiner

    public GridMap grid; // La grille de jeu associee a la tilemap de la game area

    [SerializeField]
    public Entity[] entities;
    public Entity newEntity; // La nouvelle entite a instancier
    [SerializeField]
    public Trap[] traps; // La liste des pieges instancies
    public Block[] blocks; // La liste des blocks instancies

    private Vector3 spawnPosition; // La position ou le joueur doit apparaitre
    private Vector3 spectatePosition; // La position ou le spactateur doit observer

    private Vector3 currentCell; // Cellule repere
    private Vector3 frontCell; // Cellule juste devant (au dessus) la currentCell

    public Vector3Int currentCellInt; // Cellule currentCell convertie en vectuer d'entiers avec Floor()
    public Vector3Int frontCellInt; // Cellule frontCell convertie en vectuer d'entiers avec Floor()

    public bool putTile; // Peut-on placer un Tile
    public bool putEntity; // Peut-on placer une Entity







    private void Start()
    {
        putTile = true;
        putEntity = true;
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

        // Parametres initiaux
        spawnPosition = new Vector3(startTilemap.x + Mathf.FloorToInt((grid.GetWidth() / 2)) + 0.5f, startTilemap.y + 1f, 0f);
        spectatePosition = GameObject.Find("SpectatePosition").transform.position; // On load la position de spectate dans son vecteur

        UpdateEntitiesArrays(); // Update des arrays d'entites
    }



    void Update()
    {
        Camera.main.transform.position = new Vector3((endTilemap.x + startTilemap.x) / 2, (endTilemap.y + startTilemap.y) / 2, -Mathf.Max(grid.GetWidth(), grid.GetHeight())); // La camera suit le joueur
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            Clicked();
        }
    }

    void Clicked()
    {
        Vector3 clickPosition;
        clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, Camera.main.transform.position.z));
        Vector3Int localCellPosition = new Vector3Int(Mathf.FloorToInt(clickPosition.x), Mathf.FloorToInt(clickPosition.y), 0);
        Vector3Int gridCellPosition = grid.GetLocalPosition(localCellPosition);
        if (putTile) // TODO : si on veu tplacer un tile
        {
            tilemap.SetTile(localCellPosition, drawingTile);
            grid = new GridMap(tilemap.size.x, tilemap.size.y, 1f, tilemap.origin); // TODO Adapter automatiquement
        }
        else if (putEntity) // Si on veut placer un block
        {
            if (grid.CheckGrid(gridCellPosition.x, gridCellPosition.y))
            {
                Entity entityInstance;
                if (newEntity is Trap) entityInstance = Instantiate(newEntity, localCellPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
                else entityInstance = Instantiate(newEntity, localCellPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, GameObject.Find("Blocks").transform); // Pose le nouveau block
                grid.SetValue(gridCellPosition.x, gridCellPosition.y, entityInstance);
            }
        }

    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void OnMouseDown()
    {
        Debug.Log(name);
    }

    public void UpdateEntitiesArrays()
    {
        // Liste des pièges
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        // Liste des blocks
        blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        // Liste des entites puis remplissage des entites presentes dans cette liste
        entities = new Entity[traps.Length + blocks.Length];
        traps.CopyTo(entities, 0);
        blocks.CopyTo(entities, traps.Length);
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
        foreach (Entity entity in entities) //  on vide le tableau de traps
        {
            Object.Destroy(entity.gameObject);
        }

        for (int i = 0; i < data.trapsPositions.GetLength(0); i++)
        {
            Vector3 trapPosition = new Vector3(data.trapsPositions[i,0], data.trapsPositions[i, 1], data.trapsPositions[i, 2]);
            Vector3Int gridCellPosition = grid.GetLocalPosition(trapPosition);
            Trap trapInstance;
            trapInstance = Instantiate(Resources.Load<Trap>("Prefab/LevelEntities/KillTrap"), trapPosition, Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
            grid.SetValue(gridCellPosition.x, gridCellPosition.y, trapInstance);
        }
        for (int i = 0; i < data.blocksPositions.GetLength(0); i++)
        {
            Vector3 blockPosition = new Vector3(data.blocksPositions[i, 0], data.blocksPositions[i, 1], data.blocksPositions[i, 2]);
            Vector3Int gridCellPosition = grid.GetLocalPosition(blockPosition);
            Block blockInstance;
            blockInstance = Instantiate(Resources.Load<Block>("Prefab/LevelEntities/MovableBlock"), blockPosition, Quaternion.identity, GameObject.Find("Blocks").transform); // Pose le nouveau piege
            grid.SetValue(gridCellPosition.x, gridCellPosition.y, blockInstance);
        }
        blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
    }
}

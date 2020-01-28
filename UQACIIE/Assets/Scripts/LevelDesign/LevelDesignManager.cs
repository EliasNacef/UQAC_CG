using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;


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

    [SerializeField]
    private GameObject pausePanel; // Le panel a afficher en pause



    private void Start()
    {
        putTile = true;
        putEntity = true;
        tilemap.ClearAllTiles();
        for(int i = startTilemap.x; i < endTilemap.x; i++)
        {
            for(int j = startTilemap.y; j < endTilemap.y; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), Resources.Load<TileBase>("Prefab/redTile"));
            }
        }
        

        // Parametres initiaux
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        UpdateEntitiesArrays(); // Update des arrays d'entites

        ResetGrid();
        startTilemap = tilemap.origin;
        endTilemap = tilemap.origin + new Vector3Int(grid.GetWidth(), grid.GetHeight(), 0);
        int gridSize = grid.GetHeight() * grid.GetWidth();
        for (int i = startTilemap.x - gridSize; i < endTilemap.x + gridSize; i++)
        {
            for (int j = startTilemap.y - gridSize; j < endTilemap.y + gridSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                GameObject.Find("Tilemap_Base").GetComponent<Tilemap>().SetTile(tilePosition, Resources.Load<TileBase>("Prefab/WaterfallMain"));
            }
        }
    }



    void Update()
    {
        if (grid != null) Camera.main.transform.position = new Vector3((endTilemap.x + startTilemap.x) / 2, (endTilemap.y + startTilemap.y) / 2, Mathf.Min(-12, -Mathf.Max(grid.GetWidth(), grid.GetHeight()))); // La camera suit le joueur
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            Clicked();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
    }

    void Clicked()
    {
        Vector3 clickPosition;
        clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, Camera.main.transform.position.z));
        Vector3Int localCellPosition = new Vector3Int(Mathf.FloorToInt(clickPosition.x), Mathf.FloorToInt(clickPosition.y), 0);
        if (putTile) // TODO : si on veut placer un tile
        {
            tilemap.SetTile(localCellPosition, drawingTile);
            ResetGrid();
            DrawBackground();
        }
        else if (putEntity) // Si on veut placer un block
        {
            Vector3Int gridCellPosition = grid.GetLocalPosition(localCellPosition);
            if (grid.CheckGrid(gridCellPosition.x, gridCellPosition.y))
            {
                Entity entityInstance;
                if (newEntity is Trap) entityInstance = Instantiate(newEntity, localCellPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
                else entityInstance = Instantiate(newEntity, localCellPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, GameObject.Find("Blocks").transform); // Pose le nouveau block
                grid.SetValue(gridCellPosition.x, gridCellPosition.y, entityInstance);
            }
        }
    }


    private void Pause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }


    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
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

    public void ResetGrid()
    {
        grid = new GridMap(tilemap.size.x, tilemap.size.y, 1f, tilemap.origin);
        UpdateEntitiesArrays();
        foreach (Entity entity in entities) //  on vide le tableau de traps
        {
            Destroy(entity.gameObject);
        }
        UpdateEntitiesArrays();
    }


    /// <summary>
    /// Sauvegarde le level en cours
    /// </summary>
    public void SaveLevel()
    {
        GameObject saveName = GameObject.Find("SaveName");
        SaveSystem.SaveLevel(this, saveName.GetComponent<InputField>().text); // On a sauvegarde le level
        EventSystem.current.SetSelectedGameObject(null);
    }


    /// <summary>
    /// Load le level sauvegarde
    /// </summary>
    public void LoadLevel() // TODO REMPLIR LE GRID
    {
        string fileName = FindObjectOfType<InputField>().text;
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".uqac"))
        {
            LevelData data = SaveSystem.LoadLevel(fileName);

            // Tiles
            tilemap.ClearAllTiles();
            ResetGrid();
            for (int i = 0; i < data.tilesPositions.GetLength(0); i++)
            {
                Vector3Int tilePosition = new Vector3Int(data.tilesPositions[i, 0], data.tilesPositions[i, 1], data.tilesPositions[i, 2]);
                Vector3Int gridCellPosition = tilePosition;
                //Debug.Log(data.tilesTypes[i, 0]);
                if (data.tilesTypes[i, 0] != null) tilemap.SetTile(tilePosition, Resources.Load<TileBase>("Prefab/Tiles/" + data.tilesTypes[i, 0]));
            }
            ResetGrid();


            // Traps
            for (int i = 0; i < data.trapsPositions.GetLength(0); i++)
            {
                Vector3 trapPosition = new Vector3(data.trapsPositions[i, 0], data.trapsPositions[i, 1], data.trapsPositions[i, 2]);
                Vector3Int gridCellPosition = grid.GetLocalPosition(trapPosition);
                Trap trapInstance;
                trapInstance = Instantiate(Resources.Load<Trap>("Prefab/LevelEntities/" + data.trapsTypes[i, 0]), trapPosition, Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
                grid.SetValue(gridCellPosition.x, gridCellPosition.y, trapInstance);
            }

            // Blocks
            for (int i = 0; i < data.blocksPositions.GetLength(0); i++)
            {
                Vector3 blockPosition = new Vector3(data.blocksPositions[i, 0], data.blocksPositions[i, 1], data.blocksPositions[i, 2]);
                Vector3Int gridCellPosition = grid.GetLocalPosition(blockPosition);
                Block blockInstance;
                blockInstance = Instantiate(Resources.Load<Block>("Prefab/LevelEntities/" + data.blocksTypes[i, 0]), blockPosition, Quaternion.identity, GameObject.Find("Blocks").transform); // Pose le nouveau piege
                grid.SetValue(gridCellPosition.x, gridCellPosition.y, blockInstance);
            }
            blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
            traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();

            DrawBackground();
            UpdateCamera();
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Debug.Log("File not " + fileName + " found");
        }
    }


    private void DrawBackground()
    {
        startTilemap = tilemap.origin;
        endTilemap = tilemap.origin + new Vector3Int(grid.GetWidth(), grid.GetHeight(), 0);
        int gridSize = grid.GetHeight() + grid.GetWidth();
        for (int i = startTilemap.x - gridSize; i < endTilemap.x + gridSize; i++)
        {
            for (int j = startTilemap.y - gridSize; j < endTilemap.y + gridSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                GameObject.Find("Tilemap_Base").GetComponent<Tilemap>().SetTile(tilePosition, Resources.Load<TileBase>("Prefab/WaterfallMain"));
            }
        }
    }


    private void UpdateCamera()
    {
        startTilemap = tilemap.origin;
        endTilemap = tilemap.origin + new Vector3Int(grid.GetWidth(), grid.GetHeight(), 0);
        if (grid != null) Camera.main.transform.position = new Vector3((endTilemap.x + startTilemap.x) / 2, (endTilemap.y + startTilemap.y) / 2, Mathf.Min(-12, -Mathf.Max(grid.GetWidth(), grid.GetHeight()))); // La camera suit le joueur
    }
}

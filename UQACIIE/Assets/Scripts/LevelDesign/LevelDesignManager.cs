﻿using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// Classe gestionnaire de la map. Interagit avec la map en fonction de ce qu'il s'y passe.
/// </summary>
public class LevelDesignManager : MonoBehaviour
{
    public Map map;
    public Entity newEntity;
    public bool putTile; // Peut-on placer un Tile
    public bool putEntity; // Peut-on placer une Entity

    [SerializeField]
    private GameObject pausePanel = null; // Le panel a afficher en pause



    private void Start()
    {
        putTile = true;
        putEntity = true;
        map.tilemap.ClearAllTiles();
        for(int i = map.startTilemap.x; i < map.endTilemap.x; i++)
        {
            for(int j = map.startTilemap.y; j < map.endTilemap.y; j++)
            {
                map.tilemap.SetTile(new Vector3Int(i, j, 0), Resources.Load<TileBase>("Prefab/Bleu"));
            }
        }
        

        // Parametres initiaux
        map.traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        map.blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        UpdateEntitiesArrays(); // Update des arrays d'entites

        map.ResetGrid();
        DrawBackground();
    }

    void Update()
    {
        if (map.grid != null) Camera.main.transform.position = new Vector3((map.endTilemap.x + map.startTilemap.x) / 2, (map.endTilemap.y + map.startTilemap.y) / 2, Mathf.Min(-12, -Mathf.Max(map.grid.GetWidth(), map.grid.GetHeight()) - 3)); 
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            Clicked();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
    }

    /// <summary>
    /// Décrit ce qu'il se passe lorsque l'on clique sur l'editeur de map
    /// </summary>
    void Clicked()
    {
        Vector3 clickPosition;
        clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, Camera.main.transform.position.z));
        Vector3Int localCellPosition = new Vector3Int(Mathf.FloorToInt(clickPosition.x), Mathf.FloorToInt(clickPosition.y), 0);
        TileBase currentTile = map.tilemap.GetTile(localCellPosition);
        if (putTile) // Si on veut placer un tile
        {
            if(currentTile == null)
            {
                map.tilemap.SetTile(localCellPosition, map.drawingTile);
                map.ResetGrid();
                DrawBackground();
            }
            else
            {
                map.tilemap.SetTile(localCellPosition, null);
                map.ResetGrid();
                DrawBackground();
            }
        }
        else if (putEntity) // Si on veut placer un block
        {
            Vector3Int gridCellPosition = map.grid.GetLocalPosition(localCellPosition);
            if (map.grid.CheckGrid(gridCellPosition.x, gridCellPosition.y) && currentTile != null)
            {
                Entity entityInstance;
                if (newEntity is Trap) entityInstance = Instantiate(newEntity, localCellPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
                else entityInstance = Instantiate(newEntity, localCellPosition + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, GameObject.Find("Blocks").transform); // Pose le nouveau block
                map.grid.SetValue(gridCellPosition.x, gridCellPosition.y, entityInstance);
            }
            else
            {
                if( map.grid.GetValue(gridCellPosition.x, gridCellPosition.y) != null ) Destroy(map.grid.GetValue(gridCellPosition.x, gridCellPosition.y).gameObject);
            }
        }
    }

    /// <summary>
    /// Pause ou reprendre l'édition de map
    /// </summary>
    private void Pause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    /// <summary>
    /// Savoir si la souris se trouve sur l'UI ou non
    /// </summary>
    /// <returns> Le booleen correspondant </returns>
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }


    /// <summary>
    /// Met a jours le tableau des entites de la map
    /// </summary>
    public void UpdateEntitiesArrays()
    {
        // Liste des pièges
        map.traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        // Liste des blocks
        map.blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        // Liste des entites puis remplissage des entites presentes dans cette liste
        map.entities = new Entity[map.traps.Length + map.blocks.Length];
        map.traps.CopyTo(map.entities, 0);
        map.blocks.CopyTo(map.entities, map.traps.Length);
    }



    /// <summary>
    /// Load le level sauvegarde
    /// </summary>
    public void LoadLevel()
    {
        string fileName = FindObjectOfType<InputField>().text;
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".uqac"))
        {
            LevelData data = SaveSystem.LoadLevel(fileName + ".uqac");

            // Tiles
            map.tilemap.ClearAllTiles();
            map.ResetGrid();
            for (int i = 0; i < data.tilesPositions.GetLength(0); i++)
            {
                Vector3Int tilePosition = new Vector3Int(data.tilesPositions[i, 0], data.tilesPositions[i, 1], data.tilesPositions[i, 2]);
                Vector3Int gridCellPosition = tilePosition;
                if (data.tilesTypes[i, 0] != null) map.tilemap.SetTile(tilePosition, Resources.Load<TileBase>("Prefab/Tiles/" + data.tilesTypes[i, 0]));
            }
            map.ResetGrid();


            // Traps
            for (int i = 0; i < data.trapsPositions.GetLength(0); i++)
            {
                Vector3 trapPosition = new Vector3(data.trapsPositions[i, 0], data.trapsPositions[i, 1], data.trapsPositions[i, 2]);
                Vector3Int gridCellPosition = map.grid.GetLocalPosition(trapPosition);
                Trap trapInstance;
                trapInstance = Instantiate(Resources.Load<Trap>("Prefab/LevelEntities/" + data.trapsTypes[i, 0]), trapPosition, Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
                map.grid.SetValue(gridCellPosition.x, gridCellPosition.y, trapInstance);
            }

            // Blocks
            for (int i = 0; i < data.blocksPositions.GetLength(0); i++)
            {
                Vector3 blockPosition = new Vector3(data.blocksPositions[i, 0], data.blocksPositions[i, 1], data.blocksPositions[i, 2]);
                Vector3Int gridCellPosition = map.grid.GetLocalPosition(blockPosition);
                Block blockInstance;
                blockInstance = Instantiate(Resources.Load<Block>("Prefab/LevelEntities/" + data.blocksTypes[i, 0]), blockPosition, Quaternion.identity, GameObject.Find("Blocks").transform); // Pose le nouveau piege
                map.grid.SetValue(gridCellPosition.x, gridCellPosition.y, blockInstance);
            }
            map.blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
            map.traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();

            DrawBackground();
            UpdateCamera();
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Debug.Log("File " + fileName + ".uqac not found");
        }
    }

    
    /// <summary>
    /// Dessine le background de l'editeur de map
    /// </summary>
    private void DrawBackground()
    {
        map.startTilemap = map.tilemap.origin;
        map.endTilemap = map.tilemap.origin + new Vector3Int(map.grid.GetWidth(), map.grid.GetHeight(), 0);
        int backSize = Mathf.Max(15, (map.grid.GetHeight() + map.grid.GetWidth()));
        for (int i = map.startTilemap.x - backSize; i < map.endTilemap.x + backSize; i++)
        {
            for (int j = map.startTilemap.y - backSize; j < map.endTilemap.y + backSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                GameObject.Find("Tilemap_Base").GetComponent<Tilemap>().SetTile(tilePosition, Resources.Load<TileBase>("Prefab/WaterfallMain"));
            }
        }
    }

    /// <summary>
    /// Met a jour la camera en fonction de la map que l'on dessine afin de toujours voir la map
    /// </summary>
    private void UpdateCamera()
    {
        map.startTilemap = map.tilemap.origin;
        map.endTilemap = map.tilemap.origin + new Vector3Int(map.grid.GetWidth(), map.grid.GetHeight(), 0);
        if (map.grid != null) Camera.main.transform.position = new Vector3((map.endTilemap.x + map.startTilemap.x) / 2, (map.endTilemap.y + map.startTilemap.y) / 2, Mathf.Min(-12, -Mathf.Max(map.grid.GetWidth(), map.grid.GetHeight()))); // La camera suit le joueur
    }
}

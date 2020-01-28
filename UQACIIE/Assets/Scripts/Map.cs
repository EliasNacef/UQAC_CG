using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// Classe gestionnaire de la map. Interagit avec la map en focntion de ce qu'il s'y passe.
/// </summary>
public class Map : MonoBehaviour
{

    [SerializeField]
    public Tilemap tilemap; // Tilemap de de la Game Area
    public Vector3Int startTilemap; // Position de debut de dessin (en bas a gauche)
    public Vector3Int endTilemap; // Position de fin de dessin (en haut a droite)
    public Tile drawingTile; // Tile pour dessiner


    [SerializeField]
    private Tilemap selectionMap; // Tilemap ou la case selectionnee est dessinee
    public TileBase selectionTile; // La TileBase que l'on va dessiner pour la selection
    public Vector3Int selectionRotation;
    public TileBase oldTile; // La TileBase que l'on va stocker avant de redessiner
    public Vector3Int oldPosition;
    public GridMap grid; // La grille de jeu associee a la tilemap de la game area

    [SerializeField]
    public Entity[] entities;
    [SerializeField]
    public Trap[] traps; // La liste des pieges instancies
    public Block[] blocks; // La liste des blocks instancies
    public Player[] players; // La liste des blocks instancies


    public Vector3 spawnPosition; // La position ou le joueur doit apparaitre
    public Vector3 spectatePosition; // La position ou le spactateur doit observer

    private Vector3 currentCell; // Cellule repere
    private Vector3 frontCell; // Cellule juste devant (au dessus) la currentCell
    public Vector3Int currentCellInt; // Cellule currentCell convertie en vectuer d'entiers avec Floor()
    public Vector3Int frontCellInt; // Cellule frontCell convertie en vectuer d'entiers avec Floor()


    private void Awake()
    {
        // Parametres initiaux
        selectionRotation = new Vector3Int(0, 1, 0);
        UpdateEntitiesArrays(); // Update des arrays d'entites
        oldTile = selectionMap.GetTile(frontCellInt); // Ancienne Tile initialisee
        oldPosition = frontCellInt;
    }


    /// <summary>
    /// Met en place la cellule de selection pour poser un piege : dessine une tile devant le joueur pour voir ou le piege sera place
    /// </summary>
    public void SetSelectionTile(GameObject go)
    {

        selectionMap.SetTile(oldPosition, oldTile); // Remet en place l'ancienne Tile
        UpdateAroundPosition(go);
        oldTile = selectionMap.GetTile(currentCellInt + selectionRotation); // Stocke la tile avant de la changer
        oldPosition = currentCellInt + selectionRotation;
        selectionMap.SetTile(currentCellInt + selectionRotation, selectionTile); // Dessine la tile de selection

    }

    /// <summary>
    /// Tente de placer un piege sur la position de selection du piege.
    /// </summary>
    public void SetTrap(Entity newEntity)
    {
        Vector3 positionTrap = tilemap.GetCellCenterLocal(currentCellInt + selectionRotation); // La future position du piege
        Vector3Int cellTrap = grid.GetLocalPosition(positionTrap);
        if (grid.CheckGrid(cellTrap.x, cellTrap.y))
        {
            var entityInstance = Instantiate(newEntity, positionTrap, Quaternion.identity, GameObject.Find("Traps").transform); // Pose le nouveau piege
            grid.SetValue(cellTrap.x, cellTrap.y, entityInstance);
            FindObjectOfType<AudioManager>().Play("PutTrap");
            selectionRotation = new Vector3Int(0, 1, 0);
        }
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


    public void UpdateEntitiesArrays()
    {
        // Liste des pièges
        traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        // Liste des blocks
        blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        // Liste des players
        //players = GameObject.Find("Players").GetComponentsInChildren<Player>();
        // Liste des entites puis remplissage des entites presentes dans cette liste
        entities = new Entity[traps.Length + blocks.Length]; //+ players.Length];
        traps.CopyTo(entities, 0);
        blocks.CopyTo(entities, traps.Length);
        //players.CopyTo(entities, traps.Length + blocks.Length);
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

    private void LoadDefault()
    {
        tilemap.ClearAllTiles();
        for (int i = startTilemap.x; i < endTilemap.x; i++)
        {
            for (int j = startTilemap.y; j < endTilemap.y; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), drawingTile);
            }
        }
        grid = new GridMap(tilemap.size.x, tilemap.size.y, 1f, tilemap.origin);
        Camera.main.transform.position = new Vector3((endTilemap.x + startTilemap.x) / 2, (endTilemap.y + startTilemap.y) / 2, -Mathf.Max(grid.GetWidth(), grid.GetHeight())); // La camera suit le joueur
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
    /// <summary>
    /// Load le level sauvegarde
    /// </summary>
    public void LoadLevel()
    {
        LevelData data;
        string saveName = PlayerPrefs.GetString("Save");
        if (File.Exists(Application.persistentDataPath + "/" + saveName + ".uqac"))
        {
            data = SaveSystem.LoadLevel(saveName);
        }
        else
        {
            data = SaveSystem.LoadLevel("");
        }


        // Tiles
        tilemap.ClearAllTiles();
        ResetGrid();
        for (int i = 0; i < data.tilesPositions.GetLength(0); i++)
        {
            Vector3Int tilePosition = new Vector3Int(data.tilesPositions[i, 0], data.tilesPositions[i, 1], data.tilesPositions[i, 2]);
            if (data.tilesTypes[i, 0] != null) tilemap.SetTile(tilePosition, Resources.Load<TileBase>("Prefab/Tiles/" + data.tilesTypes[i, 0]));
        }
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
        Camera.main.transform.position = new Vector3((endTilemap.x + startTilemap.x) / 2, (endTilemap.y + startTilemap.y) / 2, -(Mathf.Abs(grid.GetWidth()) + Mathf.Abs(grid.GetHeight())));
        EventSystem.current.SetSelectedGameObject(null);

    }


    public void RotateSelection()
    {
        if (selectionRotation.x == -1) selectionRotation = new Vector3Int(0, 1, 0);
        else if (selectionRotation.x == 1) selectionRotation = new Vector3Int(0, -1, 0);
        else if (selectionRotation.y == 1) selectionRotation = new Vector3Int(1, 0, 0);
        else if (selectionRotation.y == -1) selectionRotation = new Vector3Int(-1, 0, 0);
    }
}

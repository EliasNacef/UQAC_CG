using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LevelData
{
    public int[,] tilesPositions; // La liste des tiles de la map
    public float[,] trapsPositions; // La liste des positions de pieges instancies
    public float[,] blocksPositions; // La liste des positions de blocks instancies
    public string[,] tilesTypes; // La liste des types de tiles de la map
    public string[,] trapsTypes; // La liste des types  de pieges instancies
    public string[,] blocksTypes; // La liste des types de blocks instancies



    public LevelData(LevelDesignManager levelDesignManager)
    {

        int i = 0;
        int gridX = levelDesignManager.grid.GetWidth();
        int gridY = levelDesignManager.grid.GetHeight();
        int tilesSize = gridX * gridY;
        tilesPositions = new int[tilesSize, 3];
        tilesTypes = new string[tilesSize, 1];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 tilePosition = levelDesignManager.grid.GetWorldPosition(x, y);
                Vector3Int worldIntPosition = new Vector3Int(Mathf.FloorToInt(tilePosition.x), Mathf.FloorToInt(tilePosition.y), 0);
                TileBase tile = levelDesignManager.tilemap.GetTile(worldIntPosition);
                if(tile != null)
                {
                    tilesPositions[i, 0] = worldIntPosition.x;
                    tilesPositions[i, 1] = worldIntPosition.y;
                    tilesPositions[i, 2] = worldIntPosition.z;
                    tilesTypes[i, 0] = tile.name;
                    i++;
                }
            }
        }

        Trap[] traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        Block[] blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        trapsPositions = new float[traps.GetLength(0), 3];
        blocksPositions = new float[blocks.GetLength(0), 3];
        trapsTypes = new string[traps.GetLength(0), 1];
        blocksTypes = new string[blocks.GetLength(0), 1];

        i = 0;
        foreach (Trap trap in traps)
        {
            Vector3 trapPosition = trap.transform.position;
            trapsPositions[i, 0] = trapPosition.x;
            trapsPositions[i, 1] = trapPosition.y;
            trapsPositions[i, 2] = trapPosition.z;
            trapsTypes[i, 0] = trap.GetType().ToString();
            i++;
        }

        i = 0;
        foreach (Block block in blocks)
        {
            Vector3 blockPosition = block.transform.position;
            blocksPositions[i, 0] = blockPosition.x;
            blocksPositions[i, 1] = blockPosition.y;
            blocksPositions[i, 2] = blockPosition.z;
            blocksTypes[i, 0] = block.GetType().ToString();
            i++;
        }
    }



}

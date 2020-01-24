using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LevelData
{
    public float[,] trapsPositions; // La liste des positions de pieges instancies
    public float[,] blocksPositions; // La liste des positions de blocks instancies

    public LevelData(LevelDesignManager levelDesignManager)
    {

        int i = 0;
        Trap[] traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        Block[] blocks = GameObject.Find("Blocks").GetComponentsInChildren<Block>();
        trapsPositions = new float[traps.GetLength(0), 3];
        blocksPositions = new float[traps.GetLength(0), 3];

        foreach (Trap trap in traps)
        {
            Vector3 trapPosition = trap.transform.position;
            trapsPositions[i, 0] = trapPosition.x;
            trapsPositions[i, 1] = trapPosition.y;
            trapsPositions[i, 2] = trapPosition.z;
            i++;
        }

        i = 0;
        foreach (Block block in blocks)
        {
            Vector3 blockPosition = block.transform.position;
            blocksPositions[i, 0] = blockPosition.x;
            blocksPositions[i, 1] = blockPosition.y;
            blocksPositions[i, 2] = blockPosition.z;
            i++;
        }
    }



}

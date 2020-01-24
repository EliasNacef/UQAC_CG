using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class LevelData
{
    public float[,] trapsPositions; // La liste des positions de pieges instancies

    public LevelData(LevelDesignManager levelDesignManager)
    {

        int i = 0;
        Trap[] traps = GameObject.Find("Traps").GetComponentsInChildren<Trap>();
        trapsPositions = new float[traps.GetLength(0), 3];

        foreach (Trap trap in traps)
        {
            Vector3 trapPosition = trap.transform.position;
            trapsPositions[i, 0] = trapPosition.x;
            trapsPositions[i, 1] = trapPosition.y;
            trapsPositions[i, 2] = trapPosition.z;
            i++;
            
        }
    }



}

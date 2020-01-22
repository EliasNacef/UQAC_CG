using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO : modifier pour clean up codemonkey
public class GridMap
{

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private Entity[,] gridArray;

    public GridMap(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new Entity[width, height];

        bool showGrid = true;
        if (showGrid)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void EntitiesDisp()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (gridArray.GetValue(x, y) != null) Debug.Log(" Une entite est en (x : " + x + ", y : " + y + ") => " + gridArray.GetValue(x, y).ToString());
            }
        }
    }

    /// <summary>
    /// Renvoie le centre de la cellule locale
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector3Int GetLocalPosition(Vector3 worldPosition)
    {
        int x, y;
        x = Mathf.FloorToInt(((worldPosition - originPosition).x / cellSize));
        y = Mathf.FloorToInt(((worldPosition - originPosition).y / cellSize));
        return new Vector3Int(x, y, 0);

    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public bool CheckGrid(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height && gridArray[x, y] == null)
        {
            return true;
            //if (OnGridChangedEvent != null) OnGridChangedEvent(this, new OnGridValueChanged { x = x, y = y });
        }
        else return false;
    }

    public bool SetValue(int x, int y, Entity value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            Debug.Log("FullCell in : (x : " + x + " , y :" + y + " )");

            return true;
            //if (OnGridChangedEvent != null) OnGridChangedEvent(this, new OnGridValueChanged { x = x, y = y });
        }
        else return false;
    }

    public void SetValue(Vector3 worldPosition, Entity value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public Entity GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            Debug.Log("Erreur : " + "(x: " + x + ", y: " + y + ") est a l'exterieur de la grille et vous essayez d'y acceder !");
            return null;
        }
    }

    public Entity GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void MoveEntity(int x, int y, Vector3Int translation)
    {
        Entity entity = GetValue(x, y);
        if(!entity.isStatic)
        {
            GameObject go = GetValue(x, y).gameObject;
            go.transform.position += translation;
            SetValue(x + translation.x, y + translation.y , GetValue(x, y));
            SetValue(x, y, null);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe grille qui va permettre de placer des entites ou on le souhaite sur notre tilemap
/// </summary>
public class GridMap
{

    private int width; // Largeur de la grille
    private int height; // hauteur de la grille
    private float cellSize; // Taille de la cellule
    private Vector3 originPosition; // Origine
    private Entity[,] gridArray; // Tableau (grille)

    /// <summary>
    /// Cosntructeur de la grille
    /// </summary>
    /// <param name="width"> largeur </param>
    /// <param name="height"> hauteur </param>
    /// <param name="cellSize"> taille de cellule </param>
    /// <param name="originPosition"> origine de la grille </param>
    public GridMap(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new Entity[width, height];

        // Debuggeur
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

    /// <summary>
    /// Obtenir la largeur de la grille
    /// </summary>
    /// <returns> Largeur de grille </returns>
    public int GetWidth()
    {
        return width;
    }

    /// <summary>
    /// Obtenir la hauteur de la grille
    /// </summary>
    /// <returns> Hauteur de grille </returns>
    public int GetHeight()
    {
        return height;
    }

    /// <summary>
    /// Taille cellule
    /// </summary>
    /// <returns> renvoie la taille d'une cellule </returns>
    public float GetCellSize()
    {
        return cellSize;
    }

    /// <summary>
    /// Debugage qui dit ce qu'il y a sur la grille
    /// </summary>
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

    /// <summary>
    /// Recuperer la position globale en fonction des coordonnées d'une cellule
    /// </summary>
    /// <param name="x"> asbcisse </param>
    /// <param name="y"> ordonnee </param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    /// <summary>
    /// Recuperer l'abscisse et l'odonnee d'une cellule en focntion de la position globale
    /// </summary>
    /// <param name="worldPosition"> Position globale </param>
    /// <param name="x"> asbcisse </param>
    /// <param name="y"> ordonnee </param>
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }


    /// <summary>
    /// verifie qi il y a quelque chose sur la cellule en parametre
    /// </summary>
    /// <param name="x"> abscisse </param>
    /// <param name="y"> orodnnee </param>
    /// <returns></returns>
    public bool CheckGrid(int x, int y)
    {
        if (x >= 0 && y > 0 && x < width && y < height-1 && gridArray[x, y] == null)
        {
            return true;
            //if (OnGridChangedEvent != null) OnGridChangedEvent(this, new OnGridValueChanged { x = x, y = y });
        }
        return false;
    }


    /// <summary>
    /// Assigner une entite sur une cellule de la grille
    /// </summary>
    /// <param name="x"> abscisse </param>
    /// <param name="y"> ordonnee </param>
    /// <param name="value"> Veleur de l'entite </param>
    /// <returns> renvoie Succes ou Echec ? true ou false </returns>
    public bool SetValue(int x, int y, Entity value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            //Debug.Log("FullCell in : (x : " + x + " , y :" + y + " )");

            return true;
            //if (OnGridChangedEvent != null) OnGridChangedEvent(this, new OnGridValueChanged { x = x, y = y });
        }
        else return false;
    }


    /// <summary>
    /// Assigne une entite a une cellule de la grille
    /// </summary>
    /// <param name="worldPosition"> Position </param>
    /// <param name="value"> Entite a assigner </param>
    public void SetValue(Vector3 worldPosition, Entity value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    /// <summary>
    /// Renvoie l'entite en (x,y)
    /// </summary>
    /// <param name="x"> abscisse </param>
    /// <param name="y"> ordonnee </param>
    /// <returns> L'entite en (x,y) </returns>
    public Entity GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            //Debug.Log("Erreur : " + "(x: " + x + ", y: " + y + ") est a l'exterieur de la grille et vous essayez d'y acceder !");
            return null;
        }
    }

    /// <summary>
    /// Recupere la valeur de la cellule (une entite)
    /// </summary>
    /// <param name="worldPosition"> Position de la cellule</param>
    /// <returns> renvoie l'entite </returns>
    public Entity GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }


    /// <summary>
    /// Essaye de bouger l'entite en focntion de la translation a effectuer
    /// </summary>
    /// <param name="x"> Abscisse de la cellule de l'entite </param>
    /// <param name="y"> Ordonnee de la cellule de l'entite </param>
    /// <param name="translation"> Translation a effectuer </param>
    public void MoveEntity(int x, int y, Vector3Int translation)
    {
        Entity entity = GetValue(x, y);
        if(!entity.isStatic)
        {
            GameObject go = entity.gameObject;
            go.transform.position += translation;
            SetValue(x + translation.x, y + translation.y , entity);
            SetValue(x, y, null);
        }
    }
}

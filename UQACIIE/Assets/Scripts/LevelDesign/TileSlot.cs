using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TileSlot : MonoBehaviour
{
    private LevelDesignManager levelDesinManager;
    private Tile tile;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    [SerializeField]
    private bool isCurrent;

    private void Start()
    {
        levelDesinManager = GameObject.Find("Tiles").GetComponent<LevelDesignManager>(); // On load le MapManager
    }

    public void SetSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }

    public void SetColor(Color newColor)
    {
        image.color = newColor;
    }

    public void SetString(string newString)
    {
        text.text = newString;
    }

    public void SetEntity(Tile newTile)
    {
        tile = newTile;
    }


    public void SelectTrap()
    {
        levelDesinManager.drawingTile = tile;
        
    }

    private void Update()
    {
        if (isCurrent)
        {
            SetSprite(levelDesinManager.drawingTile.sprite);
            SetColor(levelDesinManager.drawingTile.color);
            SetString("Current DrawingTile");
        }
    }

}

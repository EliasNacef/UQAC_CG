using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileSlot : MonoBehaviour
{
    private LevelDesignManager levelDesignManager;
    private Tile tile;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    [SerializeField]
    private bool isCurrent;

    private void Start()
    {
        levelDesignManager = GameObject.Find("Tiles").GetComponent<LevelDesignManager>(); // On load le MapManager
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

    public void SetTile(Tile newTile)
    {
        tile = newTile;
    }


    public void Select()
    {
        if(tile != null) {
            levelDesignManager.map.drawingTile = tile;
            levelDesignManager.putEntity = false;
            levelDesignManager.putTile = true;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (isCurrent)
        {
            SetSprite(levelDesignManager.map.drawingTile.sprite);
            SetColor(levelDesignManager.map.drawingTile.color);
            SetString("Current Tile");
        }
    }

}

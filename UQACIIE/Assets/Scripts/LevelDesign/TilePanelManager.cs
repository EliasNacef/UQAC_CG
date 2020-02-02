using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


// Classe qui va mettre en place les pieges que l'on peut selectionner
public class TilePanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tileSlot;
    [SerializeField]
    private Tile[] tiles;
    private GameObject instance;

    void Start()
    {
        tiles = Resources.LoadAll<Tile>("Prefab/Tiles");
        tileSlot = Resources.Load<GameObject>("Prefab/TileSlot");
        foreach (Tile tile in tiles)
        {
            instance = Instantiate(tileSlot, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
            TileSlot slot = instance.GetComponent<TileSlot>();
            slot.SetSprite(tile.sprite);
            slot.SetColor(tile.color);
            slot.SetString(tile.name);
            slot.SetTile(tile);
        }
    }
}

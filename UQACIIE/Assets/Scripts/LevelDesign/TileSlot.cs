using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

/// <summary>
/// Classe d'un slot de tile
/// </summary>
public class TileSlot : MonoBehaviour
{
    private LevelDesignManager levelDesignManager;
    private Tile tile;
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private bool isCurrent = false;

    private void Start()
    {
        levelDesignManager = FindObjectOfType<LevelDesignManager>(); // On load le MapManager
    }

    /// <summary>
    /// Setter du sprite du slot
    /// </summary>
    /// <param name="newSprite"> Le nouveau sprite </param>
    public void SetSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }

    /// <summary>
    /// Setter de la couleur du slot
    /// </summary>
    /// <param name="newColor"> Nouvelle couleur </param>
    public void SetColor(Color newColor)
    {
        image.color = newColor;
    }

    /// <summary>
    /// Setter du nom du slot
    /// </summary>
    /// <param name="newString"> Nom du slot </param>
    public void SetString(string newString)
    {
        text.text = newString;
    }

    /// <summary>
    /// Setter du tile asssocie au slot
    /// </summary>
    /// <param name="newEntity"> Entite du slot </param>
    public void SetTile(Tile newTile)
    {
        tile = newTile;
    }

    /// <summary>
    /// Selectionner le tile du slot
    /// </summary>
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
        if (isCurrent) // Mise a jour du slot selectionne
        {
            SetSprite(levelDesignManager.map.drawingTile.sprite);
            SetColor(levelDesignManager.map.drawingTile.color);
            SetString("Sélection");
        }
    }

}

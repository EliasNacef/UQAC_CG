using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Classe d'un slot d'entity
/// </summary>
public class EntityLevelSlot : MonoBehaviour
{
    private LevelDesignManager levelDesignManager;
    private Entity entity;
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private bool isCurrent = false;

    private void Start()
    {
        levelDesignManager = FindObjectOfType<LevelDesignManager>(); // On load le manager de levels
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
    /// Setter de l'entite asssociee au slot
    /// </summary>
    /// <param name="newEntity"> Entite du slot </param>
    public void SetEntity(Entity newEntity)
    {
        entity = newEntity;
    }

    /// <summary>
    /// Selectionner l'entite du slot
    /// </summary>
    public void Select()
    {
        if (entity != null)
        {
            levelDesignManager.newEntity = entity;
            levelDesignManager.putEntity = true;
            levelDesignManager.putTile = false;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (isCurrent) // Mise a jour du slot selectionne
        {
            SetSprite(levelDesignManager.newEntity.gameObject.GetComponent<SpriteRenderer>().sprite);
            SetColor(levelDesignManager.newEntity.gameObject.GetComponent<SpriteRenderer>().color);
        }
    }

}

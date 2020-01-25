using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class EntityLevelSlot : MonoBehaviour
{
    private LevelDesignManager levelDesignManager;
    private Entity entity;
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

    public void SetEntity(Entity newEntity)
    {
        entity = newEntity;
    }


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
        if (isCurrent)
        {
            SetSprite(levelDesignManager.newEntity.gameObject.GetComponent<SpriteRenderer>().sprite);
            SetColor(levelDesignManager.newEntity.gameObject.GetComponent<SpriteRenderer>().color);
            SetString("Current Entity");
        }
    }

}

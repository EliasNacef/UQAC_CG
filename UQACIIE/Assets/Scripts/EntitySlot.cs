using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Classe d'un slot d'entity
/// </summary>
public class EntitySlot : MonoBehaviour
{
    private GameManager gameManager;
    private Entity entity;
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private bool isCurrent = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        if (entity != null ) gameManager.newEntity = entity;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (isCurrent)
        {
            SetSprite(gameManager.newEntity.gameObject.GetComponent<SpriteRenderer>().sprite);
            SetColor(gameManager.newEntity.gameObject.GetComponent<SpriteRenderer>().color);
        }
    }

}

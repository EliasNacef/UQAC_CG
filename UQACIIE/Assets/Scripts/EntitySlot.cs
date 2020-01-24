using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntitySlot : MonoBehaviour
{
    private GameManager mapManager;
    private Entity entity;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    [SerializeField]
    private bool isCurrent;

    private void Start()
    {
        mapManager = GameObject.Find("Tiles").GetComponent<GameManager>(); // On load le MapManager
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
        mapManager.newEntity = entity;
        
    }

    private void Update()
    {
        if (isCurrent)
        {
            SetSprite(mapManager.newEntity.gameObject.GetComponent<SpriteRenderer>().sprite);
            SetColor(mapManager.newEntity.gameObject.GetComponent<SpriteRenderer>().color);
            SetString("Current Trap");
        }
    }

}

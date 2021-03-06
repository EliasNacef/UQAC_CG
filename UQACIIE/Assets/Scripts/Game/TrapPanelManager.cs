﻿using UnityEngine;
using UnityEngine.EventSystems;


// Classe qui va mettre en place les pieges que l'on peut selectionner
public class TrapPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject trapSlot;
    [SerializeField]
    private Entity[] entities;
    private GameObject instance;

    void Start()
    {
        bool isMulti = FindObjectOfType<GameManager>() is MultiGameManager;
        entities = Resources.LoadAll<Entity>("Prefab/Traps");
        trapSlot = Resources.Load<GameObject>("Prefab/TrapSlot");
        foreach(Entity entity in entities)
        {
            if (!(entity is KillTrap || entity is MovableBlock) || isMulti)
            {
                instance = Instantiate(trapSlot, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                EntitySlot slot = instance.GetComponent<EntitySlot>();
                slot.SetSprite(entity.gameObject.GetComponent<SpriteRenderer>().sprite);
                slot.SetColor(entity.gameObject.GetComponent<SpriteRenderer>().color);
                slot.SetString(entity.gameObject.name);
                slot.SetEntity(entity);
            }
        }
    }

    private void Update()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

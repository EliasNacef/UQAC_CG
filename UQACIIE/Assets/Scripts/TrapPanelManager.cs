using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        entities = Resources.LoadAll<Entity>("Prefab/Traps");
        trapSlot = Resources.Load<GameObject>("Prefab/TrapSlot");
        foreach(Entity entity in entities)
        {
            instance = Instantiate(trapSlot, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("TrapPanel").transform);
            EntitySlot slot = instance.GetComponent<EntitySlot>();
            slot.SetSprite(entity.gameObject.GetComponent<SpriteRenderer>().sprite);
            slot.SetColor(entity.gameObject.GetComponent<SpriteRenderer>().color);
            slot.SetString(entity.gameObject.name);
            slot.SetEntity(entity);
        }
    }

    private void Update()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

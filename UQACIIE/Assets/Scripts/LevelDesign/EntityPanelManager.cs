using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


// Classe qui va mettre en place les pieges que l'on peut selectionner
public class EntityPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject entityLevelSlot;
    [SerializeField]
    private Entity[] entities;
    private GameObject instance;

    void Start()
    {
        entities = Resources.LoadAll<Entity>("Prefab/LevelEntities");
        entityLevelSlot = Resources.Load<GameObject>("Prefab/EntityLevelSlot");
        foreach (Entity entity in entities)
        {
            instance = Instantiate(entityLevelSlot, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
            EntityLevelSlot slot = instance.GetComponent<EntityLevelSlot>();
            slot.SetSprite(entity.gameObject.GetComponent<SpriteRenderer>().sprite);
            slot.SetColor(entity.gameObject.GetComponent<SpriteRenderer>().color);
            slot.SetString(entity.name);
            slot.SetEntity(entity);
        }
    }
}

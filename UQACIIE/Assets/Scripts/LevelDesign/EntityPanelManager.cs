using UnityEngine;

/// <summary>
/// Classe qui va mettre en place les pieges que l'on peut selectionner
/// </summary>
public class EntityPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject entityLevelSlot; // Slot entite
    [SerializeField]
    private Entity[] entities; // Les entites du panel
    private GameObject instanceEntity; 

    void Start()
    {
        entities = Resources.LoadAll<Entity>("Prefab/LevelEntities");
        entityLevelSlot = Resources.Load<GameObject>("Prefab/EntityLevelSlot");
        foreach (Entity entity in entities)
        {
            instanceEntity = Instantiate(entityLevelSlot, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
            EntityLevelSlot slot = instanceEntity.GetComponent<EntityLevelSlot>();
            slot.SetSprite(entity.gameObject.GetComponent<SpriteRenderer>().sprite);
            slot.SetColor(entity.gameObject.GetComponent<SpriteRenderer>().color);
            slot.SetString(entity.name);
            slot.SetEntity(entity);
        }
    }
}

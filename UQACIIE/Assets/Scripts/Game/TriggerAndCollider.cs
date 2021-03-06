﻿using UnityEngine;

/// <summary>
/// Classe gestionnaire des collisions dans le jeu
/// </summary>
public class TriggerAndCollider : MonoBehaviour
{
    [SerializeField]
    private Trap trap; // Entity qui peut entrer en collision avec quelque chose

    private void Start()
    {
        trap = this.GetComponent<Trap>(); // Le trap qui va s'enclencher car il est en collision avec le player
    }

    /// <summary>
    /// Decrit se qu'il se passe lors d'une collision
    /// </summary>
    /// <param name="collision"> Collider qui est entre en collision avec le trap </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponentInParent<Player>(); 
        if (entity == null) entity = collision.gameObject.GetComponent<MovableBlock>();
        if (entity == null) entity = collision.gameObject.GetComponent<PushTrap>();
        if (entity == null) entity = collision.gameObject.GetComponent<KillTrap>();
        if (trap != null && entity != null && trap.isActive)
        {
            trap.gameObject.GetComponent<SpriteRenderer>().sprite = null; // Pour activer l'animation de destruction du trap.
            trap.Activate(entity); // Activation du trap
        }
    }
}

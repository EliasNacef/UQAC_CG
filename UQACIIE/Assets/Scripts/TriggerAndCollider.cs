using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe gestionnaire des collisions dans le jeu
/// </summary>
public class TriggerAndCollider : MonoBehaviour
{
    [SerializeField]
    private Player player; // Player qui peut entrer en collision avec quelque chose

    /// <summary>
    /// Decrit se qu'il se passe lors de la collision entre un collider et le joueur
    /// </summary>
    /// <param name="collision"> Objet qui est entre en collision </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Trap trap = collision.gameObject.GetComponent<Trap>(); // Le trap qui va s'enclencher car il est en collision avec le player
        collision.gameObject.GetComponent<SpriteRenderer>().sprite = null; // Pour activer l'animation de destruction du trap.
        trap.Activate(player); // Activation du trap
    }


}

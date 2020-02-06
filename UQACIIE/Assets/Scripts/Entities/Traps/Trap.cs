using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere des pieges : les classes filles auront des effets
/// </summary>
public class Trap : Entity
{
    protected bool isActivated; // Le piege a-t-il ete declenche
    public Animator animator; // Animator du piege (l'explosion)

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // On load le MapManager
        isActivated = false; // Le piege est initialement non declenche
    }

    /// <summary>
    /// Activation du piege sur le Player player
    /// </summary>
    /// <param name="player"> Joueur qui a enclenche le piege </param>
    virtual public void Activate(Entity entity)
    {
        gameManager.nbTraps = 0;
        isActivated = true;
        animator.SetBool("isActivated", isActivated); // Active l'animation du piege
        Debug.Log("Un Trap de base a été enclenché, pas une sous classe..");
    }

    /// <summary>
    /// Va mettre un terme a l'activation du piege et le detruire
    /// </summary>
    /// <returns> Attends un certain nombre de secondes pour desactiver le piege </returns>
    protected IEnumerator Desactivate()
    {
        // On retire le trap de la grid
        Vector3Int trapPosition = gameManager.map.grid.GetLocalPosition(this.transform.position);
        if (gameManager.map.grid.GetValue(trapPosition.x, trapPosition.y) == this) gameManager.map.grid.SetValue(trapPosition.x, trapPosition.y, null); // TODO Plus propre ??
        yield return new WaitForSeconds(0.0f);
        isActivated = false;
        gameManager.map.roundTraps.Remove(this);
        Object.Destroy(this.gameObject);
    }


    public IEnumerator Hiding()
    {
        Color color = this.GetComponent<SpriteRenderer>().color;
        while (color.a > 0)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, color.a - 0.1f);
            color = this.GetComponent<SpriteRenderer>().color;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

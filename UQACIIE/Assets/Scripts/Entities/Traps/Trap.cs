using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere des pieges : les classes filles auront des effets
/// </summary>
public class Trap : Entity
{
    public Animator animator; // Animator du piege (l'explosion)

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // On load le MapManager
        isActive = true;
    }

    /// <summary>
    /// Activation du piege sur le Player player
    /// </summary>
    /// <param name="player"> Joueur qui a enclenche le piege </param>
    virtual public void Activate(Entity entity)
    {
        gameManager.nbTraps = 0;
        Debug.Log("Un Trap de base a été enclenché, pas une sous classe..");
    }

    /// <summary>
    /// Va mettre un terme a l'activation du piege et le detruire
    /// </summary>
    /// <returns> Attends un certain nombre de secondes pour desactiver le piege </returns>
    protected void Desactivate()
    {
        // On retire le trap de la grid et on le detruit
        Vector3Int trapPosition = gameManager.map.grid.GetLocalPosition(this.transform.position);
        if (gameManager.map.grid.GetValue(trapPosition.x, trapPosition.y) == this) gameManager.map.grid.SetValue(trapPosition.x, trapPosition.y, null);
        gameManager.map.trapsToHide.Remove(this);
        StartCoroutine(DestroyTrap());
    }

    public IEnumerator DestroyTrap()
    {
        animator.SetTrigger("isActivated");
        yield return new WaitForSeconds(0.1f);
        Object.Destroy(this.gameObject);
    }

    /// <summary>
    /// Cacher le piege actuel
    /// </summary>
    /// <returns> Permet d'attendre assez longtemps pour que la disparition du piege soit progressive </returns>
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

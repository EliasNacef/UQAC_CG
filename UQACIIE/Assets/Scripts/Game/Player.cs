using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    private bool isHurting; // Le joueur est-il en train de subir des degats ?
    public Animator animator; // Animator du joueur
    private int _life; // Vie du joueur


    void Start()
    {
        mapManager = GameObject.Find("Tiles").GetComponent<GameManager>(); // On load le MapManager
        isHurting = false; // Un joueur en train d'etre blesser au debut de partie
        _life = 3; // Chaque joueur commence avec 3 vies
    }




    void Update()
    {
        animator.SetBool("isHurting", isHurting);
    }


    /// <summary>
    /// Le joueur attend
    /// </summary>
    public void Wait()
    {
        GetComponent<PlayerMovement>().canMove = false; // Pendant qu'il attend, le joueur ne peut pas bouger
        StartCoroutine(Waiting()); // Coroutine gerant la fin de blessure
    }

    /// <summary>
    /// Le joueur attend donc on attend et on lui permet de bouger a nouveau
    /// </summary>
    /// <returns> Renvoie des secondes pour laisser attendre assez longtemps </returns>
    public IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
        mapManager.UpdateAroundPosition(this.gameObject);
        GetComponent<PlayerMovement>().canMove = true; // Le joueur peut a nouveau bouger
    }



    /// <summary>
    /// Blesser le joueur
    /// </summary>
    public void Hurt()
    {
        _life--; // Le joueur perd une vie
        isHurting = true; // Le joueur est en train d'etre blesse
        GetComponent<PlayerMovement>().canMove = false; // Pendant sa blessure, le joueur ne peut pas bouger
        animator.SetBool("isHurting", isHurting); // Animator mis a jour (en train d'etre blesse)
        StartCoroutine(Hurting()); // Coroutine gerant la fin de blessure
    }



    /// <summary>
    /// Le joueur est en train d'etre blessé donc on update sa position et on lui permet de bouger a nouveau
    /// </summary>
    /// <returns> Renvoie des secondes pour laisser les animations assez longtemps </returns>
    public IEnumerator Hurting()
    {
        yield return new WaitForSeconds(1f);
        isHurting = false; // Fin de blessure (animation)
        yield return new WaitForSeconds(0.35f);
        mapManager.UpdatePlayersPositions(); // Update des positions (reset)
        GetComponent<PlayerMovement>().canMove = true; // Le joueur peut a nouveau bouger
    }

    /// <summary>
    /// Getter et Setter de la vie du joueur 
    /// </summary>
    public int Life
    {
        get { return _life; }
        set { _life = value; }
    }
}

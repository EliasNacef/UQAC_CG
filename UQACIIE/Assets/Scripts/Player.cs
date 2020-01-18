using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private MapManager map;
    private bool isHurting;
    public Animator animator;
    private int _life;
    public bool hasLost;

    // Start is called before the first frame update
    void Start()
    {
        isHurting = false;
        _life = 3; // Debut de partie à 0
        hasLost = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isHurting", isHurting);
    }

    public void Hurt()
    {
        isHurting = true;
        hasLost = true;
        GetComponent<PlayerMovement>().canMove = false;
        animator.SetBool("isHurting", isHurting);
        StartCoroutine(Hurting());
    }

    public IEnumerator Hurting()
    {
        yield return new WaitForSeconds(1f);
        isHurting = false;
        yield return new WaitForSeconds(0.35f);
        map.UpdatePositions();
        GetComponent<PlayerMovement>().canMove = true;
        //map.EndRound();
        // TODO Faire sauter le joueur jusqu'à la plateforme et passer au joueur suivant
    }

    public int Life
    {
        get { return _life; }
        set { _life = value; }
    }
}

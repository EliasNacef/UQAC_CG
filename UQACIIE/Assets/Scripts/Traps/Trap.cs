using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    protected bool isActivated;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isActivated", isActivated);
    }


    virtual public void Activate(Player player)
    {
        isActivated = true;
        animator.SetBool("isActivated", isActivated);
        Debug.Log("Un Trap de base a été enclenché, pas une sous classe..");
    }


}
